using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IEventSink
    {
        ValueTask Send<T>(T e) where T : Event;
    }
}