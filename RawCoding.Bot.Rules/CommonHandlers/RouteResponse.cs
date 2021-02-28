using System;
using System.Threading.Tasks;
using MoistBot.Models;
using RawCoding.Bot.Rules.Twitch;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class RouteResponse : MessageHandler<Respond>
    {
        protected override ValueTask Handle(Respond msg)
        {
            if (ReferenceEquals(MessageContext.SourceType, Constants.Sources.TwitchChat))
            {
                return msg.Private
                    ? Broadcast(new SendTwitchPrivateMessage(msg.Customer, msg.Message))
                    : Broadcast(new SendTwitchPublicMessage(MessageContext.SourceId, msg.Message));
            }

            return ValueTask.CompletedTask;
        }
    }
}