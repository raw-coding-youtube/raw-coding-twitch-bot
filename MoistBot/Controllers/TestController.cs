using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace MoistBot.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<TwitchHub> _twitchHub;

        public TestController(
            IWebHostEnvironment env,
            IHubContext<TwitchHub> twitchHub)
        {
            _env = env;
            _twitchHub = twitchHub;
        }

        [HttpGet("follow")]
        public async Task<IActionResult> TestFollow()
        {
            if (_env.IsDevelopment())
                await _twitchHub.Clients.All.SendAsync("follow", "--TEST NAME--");

            return Ok();
        }

        [HttpGet("sub")]
        public async Task<IActionResult> TestSub(int count)
        {
            if (_env.IsDevelopment())
            {

                await _twitchHub.Clients.All.SendAsync("sub", new
                {
                    Username = "--TEST NAME--",
                    Total = count,
                    Tier = "GOD",
                });
            }

            return Ok();
        }
    }
}