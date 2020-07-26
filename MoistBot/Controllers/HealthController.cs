using System;
using Microsoft.AspNetCore.Mvc;

namespace MoistBot.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private static readonly long Time = DateTime.UtcNow.Ticks;

        [HttpGet]
        public string Index()
        {
            return Time.ToString();
        }
    }
}