using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MoistBot.Models
{
    public class MessageProcessingContext : IMessageSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Channel<Message> _eventChannel;

        public MessageProcessingContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventChannel = Channel.CreateUnbounded<Message>();
        }

        public ValueTask Send<T>(T e) where T : Message => _eventChannel.Writer.WriteAsync(e);

        public async ValueTask Start(CancellationToken cancellationToken)
        {
            while (await _eventChannel.Reader.WaitToReadAsync(cancellationToken))
            {
                try
                {
                    var e = await _eventChannel.Reader.ReadAsync(cancellationToken);
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var handlers = scope.ServiceProvider
                            .GetServices(typeof(MessageHandler<>).MakeGenericType(e.GetType()));

                        foreach (var h in handlers)
                        {
                            await (h as MessageHandler).InternalHandle(e);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}