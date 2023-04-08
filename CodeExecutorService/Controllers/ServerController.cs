using CodeExecutorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeExecutorService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<ServerController> _logger;

        public ServerController
        (ILogger<ServerController> logger)
        { 
            _logger = logger;
        }


        [HttpGet]
        public IActionResult ServerStatus()
        {
            return Ok();
        }

        [HttpGet("languages")]
        public ActionResult<IEnumerable<Language>> SuportedLanguage()
        {
            IEnumerable<Language> languages;
            languages = new List<Language>()
            { 
                new("Python"),
                new("Shell"),
            };
            return Ok(languages);
        }

    }
}