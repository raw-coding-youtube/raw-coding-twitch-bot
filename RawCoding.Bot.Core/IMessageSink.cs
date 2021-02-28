using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IMessageSink
    {
        ValueTask Send<T>(T e) where T : Message;
    }
}