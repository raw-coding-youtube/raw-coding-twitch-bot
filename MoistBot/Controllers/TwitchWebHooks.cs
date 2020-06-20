using Microsoft.AspNetCore.Mvc;
using TwitchLib.Api;

namespace MoistBot.Controllers
{
    [ApiController]
    [Route("api/twitch-web-hooks")]
    public class TwitchWebHooks : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}