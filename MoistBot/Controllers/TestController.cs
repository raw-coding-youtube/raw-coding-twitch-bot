using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using MoistBot.EventEmitting;

namespace MoistBot.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private ChannelWriter<EventPackage> _eventWriter;

        public TestController(
            IWebHostEnvironment env,
            Channel<EventPackage> eventChannel)
        {
            _env = env;
            _eventWriter = eventChannel.Writer;
        }

        [HttpGet("follow")]
        public async Task<IActionResult> TestFollow()
        {
            if (_env.IsDevelopment())
                await _eventWriter.WriteAsync(new EventPackage
                {
                    Target = Targets.Follow,
                    Attributes = new
                    {
                        DisplayName = "Test Follower!"
                    },
                    DisplayTime = 6000
                });

            return Ok();
        }

        [HttpGet("sub")]
        public async Task<IActionResult> TestSub(int count)
        {
            if (_env.IsDevelopment())
                await _eventWriter.WriteAsync(new EventPackage {
                    Target = Targets.Subscribe,
                    Attributes = new
                    {
                        TwitchUsername = "Test User",
                        StreakMonths = 2,
                        TotalMonths = count,
                    },
                    DisplayTime = 8000
                });

            return Ok();
        }
    }
}