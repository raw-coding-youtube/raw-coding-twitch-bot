using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace RawCoding.Bot.Controllers
{
    public class Image : Controller
    {
        [HttpGet("image/{name}")]
        public IActionResult Index(string name, [FromServices] IWebHostEnvironment env)
        {
            var mime = name.Split(".")[1];
            var stream = env.WebRootFileProvider.GetFileInfo(name).CreateReadStream();
            return new FileStreamResult(stream, $"image/{mime}");
        }
    }
}