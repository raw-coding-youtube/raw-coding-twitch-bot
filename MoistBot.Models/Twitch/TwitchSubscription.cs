using System;
using MoistBot.Models.Twitch.Enums;

namespace MoistBot.Models.Twitch
{
    public class TwitchSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public TwitchUser User { get; set; }
        public string TwitchUsername { get; set; }
        public DateTime Time { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public string SubscriptionPlanName { get; set; }
        public int TotalMonths { get; set; }
        public int StreakMonths { get; set; }
        public string Context { get; set; }
    }
}