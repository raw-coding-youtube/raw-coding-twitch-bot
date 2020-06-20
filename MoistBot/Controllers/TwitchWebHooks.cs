using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwitchLib.Api;

namespace MoistBot.Controllers
{
    [ApiController]
    [Route("api/twitch-web-hooks")]
    public class TwitchWebHooks : ControllerBase
    {
        private readonly ILogger<TwitchWebHooks> _logger;

        public TwitchWebHooks(ILogger<TwitchWebHooks> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync();
            _logger.LogInformation(payload);
            return Ok();
        }
    }

    public class Payload
    {
        public Dictionary<string, string> Data { get; set; }
    }
}