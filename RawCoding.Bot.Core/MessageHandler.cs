using System.Data;
using System.Threading.Tasks;

namespace MoistBot.Models
{
    public abstract class MessageHandler
    {
        internal abstract ValueTask<Message> InternalHandle(Message msg);
    }

    public abstract class MessageHandler<TIn> : MessageHandler
        where TIn : Message
    {
        internal override ValueTask<Message> InternalHandle(Message msg) => Handle((TIn) msg);
        protected abstract ValueTask<Message> Handle(TIn msg);
    }
}