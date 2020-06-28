using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoistBot.Database;
using MoistBot.Infrastructure;
using MoistBot.Services;
using VueCliMiddleware;

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
            services.AddSpaStaticFiles(opt => opt.RootPath = "client/dist");
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapHub<TwitchHub>("/hub/twitch");

                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "client" },
                    npmScript: _env.IsDevelopment() ? "serve" : null,
                    regex: "Compiled successfully",
                    forceKill: true
                );
            });
        }
    }
}