using System.Threading.Tasks;

namespace MoistBot.Models
{
    public interface IAction
    {
        Task SendMessage(Message msg);
    }
}