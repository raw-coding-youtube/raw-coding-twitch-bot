using System.Reflection;
using System.Threading.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoistBot;
using MoistBot.Models;
using RawCoding.Bot.Data;
using RawCoding.Bot.EventEmitting;
using RawCoding.Bot.Rules.Twitch;
using RawCoding.Bot.Rules.Twitch.Sources;

namespace RawCoding.Bot
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TwitchSettings>(_config.GetSection(TwitchSettings.Name));

            services.AddSingleton(_ => Channel.CreateUnbounded<EventPackage>());
            services.AddHostedService<EventDispatcher>();

            services.AddRawCodingBot(typeof(TwitchChatBot).Assembly);

            services.AddSignalR();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapHub<TwitchHub>("/hub/twitch");
            });
        }
    }
}