using System.Threading.Tasks;

namespace MoistBot.Models
{
    public abstract class MessageHandler
    {
        internal IMessageContextSink Sink { get; set; }
        public MessageContext MessageContext { get; internal set; }
        internal abstract ValueTask InternalHandle(MessageContext ctx);
        protected ValueTask Broadcast(Message message) => Sink.Send(MessageContext with {Message = message});
    }

    public abstract class MessageHandler<TIn> : MessageHandler
        where TIn : Message
    {
        internal override ValueTask InternalHandle(MessageContext ctx)
        {
            MessageContext = ctx;
            return Handle((TIn) ctx.Message);
        }

        protected abstract ValueTask Handle(TIn msg);
    }
}