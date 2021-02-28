using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RawCoding.Bot.Controllers
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

        [HttpGet]
        public string Index()
        {
            return Request.Query.TryGetValue("hub.challenge", out var v) ? v.ToString() : "";
        }

        [HttpPost]
        public async Task<IActionResult> Receive()
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