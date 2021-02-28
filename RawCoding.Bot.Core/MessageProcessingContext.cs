using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MoistBot.Models
{
    public class MessageProcessingContext : IMessageSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Channel<MessageContext> _eventChannel;

        public MessageProcessingContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventChannel = Channel.CreateUnbounded<MessageContext>();
        }

        public ValueTask Send(MessageContext messageContext) => _eventChannel.Writer.WriteAsync(messageContext);

        public async Task Start(CancellationToken cancellationToken)
        {
            while (await _eventChannel.Reader.WaitToReadAsync(cancellationToken))
            {
                try
                {
                    var msgCtx = await _eventChannel.Reader.ReadAsync(cancellationToken);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var handlers = scope.ServiceProvider
                            .GetServices(typeof(MessageHandler<>).MakeGenericType(msgCtx.Message.GetType()));

                        await Task.WhenAll(handlers.Select(h => ProcessMessage((MessageHandler) h, msgCtx)));
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        private async Task ProcessMessage(MessageHandler handler, MessageContext context)
        {
            var result = await handler.InternalHandle(context.Message);
            if (result is NoOp) return;
            await Send(context with {Message = result});
        }
    }
}