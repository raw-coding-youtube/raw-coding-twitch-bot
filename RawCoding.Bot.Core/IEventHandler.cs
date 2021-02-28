using System.Threading.Tasks;

namespace MoistBot.Models
{
    public abstract class EventHandler
    {
        internal abstract Task InternalHandle(Event e);
    }

    public abstract class ProcessEvent<T> : EventHandler
        where T : Event
    {
        internal override Task InternalHandle(Event e) => Process((T) e);
        protected abstract Task Process(T e);
    }
}