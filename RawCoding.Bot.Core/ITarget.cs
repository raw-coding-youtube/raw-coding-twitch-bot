using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface ITarget
    {
        Task SendMessage(Message msg);
    }
}