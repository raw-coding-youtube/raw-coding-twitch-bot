using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IMessageContextSink
    {
        ValueTask Send(MessageContext messageContext);
    }
}