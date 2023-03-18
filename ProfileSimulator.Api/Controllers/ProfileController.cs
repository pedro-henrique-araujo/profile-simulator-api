using Microsoft.AspNetCore.Mvc;

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
            var openAiApiKey = _configuration.GetValue<string>("OpenAiApiKey");

            var profileGenerator = new ProfileGenerator(openAiApiKey);

            var profile = await profileGenerator.GenerateProfile();

            return Ok(profile);
        }
    }
}
