using MoistBot.Models.Twitch;

namespace MoistBot.Models
{
    public class User
    {
        public string Id { get; set; }
        public TwitchUser TwitchUser { get; set; }
    }
}