using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MoistBot.Models;

namespace RawCoding.Bot.EventEmitting
{
    public class MessageProcessingService : BackgroundService
    {
        private readonly MessageContextProcessingContext _messageContextProcessingContext;

        public MessageProcessingService(MessageContextProcessingContext messageContextProcessingContext)
        {
            _messageContextProcessingContext = messageContextProcessingContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
            _messageContextProcessingContext.Start(stoppingToken);
    }
}