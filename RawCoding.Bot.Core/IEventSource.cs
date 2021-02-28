using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IEventSource
    {
        ValueTask Register(IEventSink eventSink);
    }
}