using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace ProfileSimulator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private IConfiguration _configuration;

        public ProfileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var namesAsJson = System.IO.File.ReadAllText("names.json");

            var names = JsonSerializer.Deserialize<Dictionary<string, string[]>>(namesAsJson);

            var random = new Random();

            var profile = new Profile();

            var gender = random.Next(2);
            var possibleFirstNamesKey = gender == 0 ? "male_names" : "female_names";
            var possibleLastNamesKey = "last_names";

            var possibleFirstNames = names[possibleFirstNamesKey];
            var possibleLastNames = names[possibleLastNamesKey];

            var firstNameKey = random.Next(possibleFirstNames.Length);
            var lastNameKey = random.Next(possibleLastNames.Length);

            var firstName = possibleFirstNames[firstNameKey];
            var lastName = possibleLastNames[lastNameKey];

            var age = random.Next(70);

            var yearOfBirth = DateTime.Today.Year - age - 1;
            var monthOfBirth = random.Next(12) + 1;
            var dayOfBirth = random.Next(DateTime.DaysInMonth(yearOfBirth, monthOfBirth)) + 1;
            var dateOfBirth = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);

            var email = lastName.ToLower() + '.' + firstName.ToLower() + "@fmail.com";

            profile.FirstName = firstName;
            profile.LastName = lastName;
            profile.Gender = gender;
            profile.DateOfBirth = dateOfBirth;
            profile.Email = email;

            if (age < 4)
            {
                profile.Occupation = "Toddler";
            }
            else if (age < 6)
            {
                profile.Occupation = "Ealy Childhood Education";
            }
            else if (age < 9)
            {
                profile.Occupation = "Primary School";
            }
            else if (age < 14)
            {
                profile.Occupation = "Secondary School";
            }
            else if (age < 18)
            {
                profile.Occupation = "High School";
            }
            else if (age < 22)
            {
                profile.Occupation = "Student";
            }
            else if (age < 70)
            {
                var professions = names["professions"];
                var professionKey = random.Next(professions.Length);
                profile.Occupation = professions[professionKey];
            }
            else
            {
                profile.Occupation = "Retiree";
            }

            var webClient = new HttpClient();
            var openAiApiKey = _configuration.GetValue<string>("OpenAiApiKey");
            webClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + openAiApiKey);
            var genderText = gender == 0 ? "male" : "female";
            var profileImageGenerationPrompt = $"{age} year old {genderText} person face or selfie smiling photo";
            var requestParams = new { prompt = profileImageGenerationPrompt, n = 1, size = "256x256" };
            var requestParamsAsJson = JsonSerializer.Serialize(requestParams);
            var requestContent = new StringContent(requestParamsAsJson, null, "application/json");
            requestContent.Headers.ContentType.CharSet = string.Empty;
            var response = await webClient.PostAsync("https://api.openai.com/v1/images/generations", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var resultAsString = await response.Content.ReadAsStringAsync();
                var imageGenerationResultJObject = JObject.Parse(resultAsString);
                var data = imageGenerationResultJObject["data"];
                var firstResult = data[0];
                var url = firstResult["url"].ToString();
                profile.ProfileImageUrl = url;
            }


            return Ok(profile);
        }
    }

    public class ImageGenerationResult
    {
        public object data;
    }

    public class ImageResultUrl
    {
        public string Url { get; set; }
    }

    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Occupation { get; internal set; }
        public string? ProfileImageUrl { get; internal set; }
    }
}
