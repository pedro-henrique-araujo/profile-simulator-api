using Newtonsoft.Json.Linq;
using ProfileSimulator.Api.Data;
using System.Text.Json;

namespace ProfileSimulator.Api
{
    public class ProfileGenerator
    {
        private string _openAiApiKey;
        private Profile _profile;
        private PersonDataGenerator? _dataGenerator;
        private Random _random;
        private Dictionary<string, string[]>? _names;
        private int _age;

        public ProfileGenerator(string openAiApiKey)
        {
            _openAiApiKey = openAiApiKey;
            _random = new();
            var namesAsJson = File.ReadAllText("names.json");
            _names = JsonSerializer.Deserialize<Dictionary<string, string[]>>(namesAsJson);
            _profile = new();
        }

        public async Task<Profile> GenerateProfile()
        {
            InitializeANewProfile();
            _dataGenerator = new PersonDataGenerator(_profile);

            _dataGenerator.PickAGender();
            _dataGenerator.PickAFirstName();
            _dataGenerator.PickALastName();
            _dataGenerator.PickADateOfBirth();
            _dataGenerator.PickAnEmail();
            GenerateTheOcupation();
            await GenerateTheProfileImage();

            return _profile;
        }

        private void InitializeANewProfile()
        {
            _profile = new();
        }       

        private async Task GenerateTheProfileImage()
        {
            var webClient = new HttpClient();
            webClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _openAiApiKey);
            var genderText = _profile.Gender == 0 ? "male" : "female";
            var profileImageGenerationPrompt = $"{_age} year old {genderText} person face or selfie smiling photo";
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
                _profile.ProfileImageUrl = url;
            }
        }

        private void GenerateTheOcupation()
        {
            _age = _dataGenerator.GetAge();
            if (_age < 4)
            {
                _profile.Occupation = "Toddler";
            }
            else if (_age < 6)
            {
                _profile.Occupation = "Ealy Childhood Education";
            }
            else if (_age < 9)
            {
                _profile.Occupation = "Primary School";
            }
            else if (_age < 14)
            {
                _profile.Occupation = "Secondary School";
            }
            else if (_age < 18)
            {
                _profile.Occupation = "High School";
            }
            else if (_age < 22)
            {
                _profile.Occupation = "Student";
            }
            else if (_age < 70)
            {
                var professions = _names["professions"];
                var professionKey = _random.Next(professions.Length);
                _profile.Occupation = professions[professionKey];
            }
            else
            {
                _profile.Occupation = "Retiree";
            }
        }
    }
}
