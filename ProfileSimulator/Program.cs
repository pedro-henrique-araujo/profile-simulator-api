using System.Reflection;
using System.Text.Json;
using System.Text;

var gender = 0;
var age = 15;

var webClient = new HttpClient();
webClient.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
var genderText = gender == 0 ? "male" : "female";
var profileImageGenerationPrompt = $"{age} year old {genderText} person face or selfie photo";
var requestParams = new { prompt = profileImageGenerationPrompt, n = 1, size = "1024x1024" };
var requestParamsAsJson = JsonSerializer.Serialize(requestParams);
var requestContent = new StringContent(requestParamsAsJson, null, "application/json");
requestContent.Headers.ContentType.CharSet = string.Empty;
var response = await webClient.PostAsync("https://api.openai.com/v1/images/generations", requestContent);
string responseContent = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Response: {responseContent}");
//using System.Net.Http.Headers;
//using System.Text;

//string apiEndpoint = "https://api.openai.com/v1/images/generations";

//string apiKey = "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";
//string prompt = "a cat sitting on a couch";
//string model = "image-alpha-001";
//int batchSize = 1;
//int width = 512;
//int height = 512;
//int responseTimeout = 5000;

//HttpClient httpClient = new HttpClient();
//httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
//httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


//string requestBody = $@"{{
//            ""model"": ""{model}"",
//            ""prompt"": ""{prompt}"",
//            ""num_images"": {batchSize},
//            ""size"": ""{width}x{height}"",
//            ""response_format"": ""url""
//        }}";

//var requestContent = new StringContent(requestBody, null, "application/json");
//requestContent.Headers.ContentType.CharSet = string.Empty;

//HttpResponseMessage response = await httpClient.PostAsync(apiEndpoint, requestContent);

//if (response.IsSuccessStatusCode)
//{
//    string responseContent = await response.Content.ReadAsStringAsync();
//    Console.WriteLine($"Response: {responseContent}");
//}
//else
//{
//    Console.WriteLine($"Failed with status code: {response.StatusCode}");
//    string responseContent = await response.Content.ReadAsStringAsync();
//    Console.WriteLine($"Response: {responseContent}");
//}

