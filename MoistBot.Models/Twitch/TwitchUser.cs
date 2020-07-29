using System;
using System.Collections.Generic;
using MoistBot.Models.Twitch.Enums;

namespace MoistBot.Models.Twitch
{
    public class TwitchUser
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public bool Followed { get; set; }

        public IList<TwitchSubscription> Subscriptions { get; set; } = new List<TwitchSubscription>();
    }
}