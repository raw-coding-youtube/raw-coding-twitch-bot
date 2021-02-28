using System;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.Twitch
{
    public record TwitchMessage(string Message) : Message;

    public record ReceivedTwitchMessage(string Channel, string Username, string Message) : TwitchMessage(Message);

    public record SendTwitchPublicMessage(string Channel, string Message) : TwitchMessage(Message);

    public record SendTwitchPrivateMessage(string Username, string Message) : TwitchMessage(Message);

    public record TwitchFollow(string Username) : Message;

    public record TwitchSubscription(
        string UserId,
        string TwitchUsername,
        DateTime Time,
        int TotalMonths,
        int StreakMonths,
        string Context
    ) : Message;
}