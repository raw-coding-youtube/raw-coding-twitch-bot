using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class IdentifyCommand : MessageHandler<CustomerMessage>
    {
        protected override ValueTask<Message> Handle(CustomerMessage msg)
        {
            if (string.IsNullOrEmpty(msg.Message)) return Message.NoOpTask;

            if (!msg.Message.TrimStart().StartsWith('!')) return Message.NoOpTask;

            return Message.TaskFrom(new ExecuteCommand(msg.Author, msg.Message));
        }
    }
}