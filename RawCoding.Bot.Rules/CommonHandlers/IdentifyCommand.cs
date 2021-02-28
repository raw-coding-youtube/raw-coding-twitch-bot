using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class IdentifyCommand : MessageHandler<CustomerMessage>
    {
        protected override ValueTask Handle(CustomerMessage msg)
        {
            if (string.IsNullOrEmpty(msg.Message)) return ValueTask.CompletedTask;

            if (!msg.Message.TrimStart().StartsWith('!')) return ValueTask.CompletedTask;

            return Broadcast(new ExecuteCommand(msg.Author, msg.Message));
        }
    }
}