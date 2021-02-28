using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class IdentifyCommand : MessageHandler<CustomerMessage>
    {
        private readonly IMessageSink _sink;

        public IdentifyCommand(IMessageSink sink)
        {
            _sink = sink;
        }

        protected override ValueTask Handle(CustomerMessage msg)
        {
            if (string.IsNullOrEmpty(msg.Message)) return ValueTask.CompletedTask;

            if (!msg.Message.TrimStart().StartsWith('!')) return ValueTask.CompletedTask;

            return _sink.Send(new ExecuteCommand(msg.Author, msg.Message));
        }
    }
}