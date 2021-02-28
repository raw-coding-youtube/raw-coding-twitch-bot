using System.Threading.Tasks;

namespace MoistBot.Models
{
    public abstract class MessageHandler
    {
        internal abstract ValueTask InternalHandle(Message msg);
    }

    public abstract class MessageHandler<T> : MessageHandler
        where T : Message
    {
        internal override ValueTask InternalHandle(Message msg) => Handle((T) msg);
        protected abstract ValueTask Handle(T msg);
    }
}