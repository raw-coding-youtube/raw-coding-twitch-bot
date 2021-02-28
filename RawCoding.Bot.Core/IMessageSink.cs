using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IMessageSink
    {
        ValueTask Send(MessageContext messageContext);
    }
}