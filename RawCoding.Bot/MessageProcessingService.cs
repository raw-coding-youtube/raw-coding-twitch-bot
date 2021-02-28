using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MoistBot.Models;

namespace RawCoding.Bot.EventEmitting
{
    public class MessageProcessingService : BackgroundService
    {
        private readonly MessageProcessingContext _messageProcessingContext;

        public MessageProcessingService(MessageProcessingContext messageProcessingContext)
        {
            _messageProcessingContext = messageProcessingContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
            _messageProcessingContext.Start(stoppingToken);
    }
}