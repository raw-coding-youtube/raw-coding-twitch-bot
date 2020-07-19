namespace MoistBot.Infrastructure
{
    public class TwitchSettings
    {
        public const string Name = nameof(TwitchSettings);
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
    }
}