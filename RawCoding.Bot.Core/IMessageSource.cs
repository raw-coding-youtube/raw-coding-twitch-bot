using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IMessageSource
    {
        ValueTask Register(IMessageSink messageSink);
    }
}