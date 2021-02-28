using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MoistBot.Models
{
    public class EventProcessingContext : IEventSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Channel<Event> _eventChannel;

        public EventProcessingContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventChannel = Channel.CreateUnbounded<Event>();
        }

        public ValueTask Send<T>(T e) where T : Event => _eventChannel.Writer.WriteAsync(e);

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
                            .GetServices(typeof(ProcessEvent<>).MakeGenericType(e.GetType()));

                        foreach (var h in handlers)
                        {
                            await (h as EventHandler).InternalHandle(e);
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