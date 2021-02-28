using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace RawCoding.Bot.EventEmitting
{
    public class EventDispatcher : BackgroundService
    {
        private readonly IHubContext<TwitchHub> _hubContext;

        public EventDispatcher(
            IHubContext<TwitchHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    var package = await _eventReader.ReadAsync(stoppingToken);
                    await _hubContext.Clients.All.SendAsync("HandleEvent", package, stoppingToken);
                    await Task.Delay(package.DisplayTime, stoppingToken);
                }
                catch { }
            }
        }
    }
}