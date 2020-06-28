using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoistBot.Database;
using MoistBot.Infrastructure;
using MoistBot.Services;

namespace MoistBot
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
            services.Configure<TwitchSettings>(_config);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("Dev");
            });

            services.AddHttpClient()
                    .AddSingleton<TwitchPubSubService>()
                    .AddScoped<RegisterUserAction>();

            services.AddSignalR();
            services.AddControllers();
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();

                endpoints.MapHub<TwitchHub>("/hub/twitch");
            });
        }
    }
}