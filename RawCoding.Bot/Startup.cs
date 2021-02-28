using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoistBot.Models;
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
            services.AddRawCodingBot(typeof(TwitchChatBot), typeof(Startup));
            services.AddHostedService<MessageProcessingService>();

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