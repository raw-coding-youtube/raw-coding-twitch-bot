using System.Threading.Tasks;
using MoistBot.Models;
using RawCoding.Bot.Rules.Twitch.Sources;

namespace RawCoding.Bot.Rules.Twitch.Handlers
{
    public class SendTwitchPrivateMessageHandler : MessageHandler<SendTwitchPrivateMessage>
    {
        private readonly TwitchChatBot _twitchChatBot;

        public SendTwitchPrivateMessageHandler(TwitchChatBot twitchChatBot)
        {
            _twitchChatBot = twitchChatBot;
        }

        protected override ValueTask Handle(SendTwitchPrivateMessage msg)
        {
            _twitchChatBot.Client.SendWhisper(msg.Username, msg.Message);
            return ValueTask.CompletedTask;
        }
    }
}