using System.Threading.Tasks;
using MoistBot.Models;
using RawCoding.Bot.Rules.Twitch.Sources;

namespace RawCoding.Bot.Rules.Twitch.Handlers
{
    public class SendTwitchPublicMessageHandler : MessageHandler<SendTwitchPublicMessage>
    {
        private readonly TwitchChatBot _twitchChatBot;

        public SendTwitchPublicMessageHandler(TwitchChatBot twitchChatBot)
        {
            _twitchChatBot = twitchChatBot;
        }

        protected override ValueTask<Message> Handle(SendTwitchPublicMessage msg)
        {
            _twitchChatBot.Client.SendMessage(msg.Channel, msg.Message);
            return Message.NoOpTask;
        }
    }
}