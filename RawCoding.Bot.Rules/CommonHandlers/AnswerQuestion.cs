using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class AnswerQuestion : MessageHandler<CustomerMessage>
    {
        protected override ValueTask<Message> Handle(CustomerMessage msg)
        {
            if (string.IsNullOrEmpty(msg.Message)) return ValueTask.FromResult((Message) new Noop());

            if (!msg.Message.TrimStart().StartsWith('!')) return ValueTask.FromResult((Message) new Noop());

            return ValueTask.FromResult((Message) new ExecuteCommand(msg.Author, msg.Message));
        }
    }
}