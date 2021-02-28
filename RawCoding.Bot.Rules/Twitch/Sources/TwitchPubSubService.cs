using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoistBot.Models;
using TwitchLib.Api;
using TwitchLib.PubSub;

namespace RawCoding.Bot.Rules.Twitch.Sources
{
    public class TwitchPubSubService : IMessageSource
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<TwitchPubSubService> _logger;
        private readonly TwitchSettings _twitchSettings;
        private readonly TwitchPubSub _pubsub;

        public TwitchPubSubService(
            IOptionsMonitor<TwitchSettings> optionsMonitor,
            IServiceProvider provider,
            ILogger<TwitchPubSubService> logger)
        {
            _provider = provider;
            _logger = logger;
            _twitchSettings = optionsMonitor.CurrentValue;
            _pubsub = new();
        }

        private string AccessToken { get; set; }

        public ValueTask Register(IMessageSink messageSink)
        {
            var api = new TwitchAPI();
            api.Settings.ClientId = _twitchSettings.ClientId;
            api.Settings.Secret = _twitchSettings.ClientSecret;
            AccessToken = api.V5.Root.GetAccessToken();
            // var client = _factory.CreateClient();
            // var url = $"https://id.twitch.tv/oauth2/token?grant_type=client_credentials&client_id={_twitchSettings.ClientId}&client_secret={_twitchSettings.ClientSecret}&scope=user_read";
            // var kraken_rul = "https://api.twitch.tv/kraken/user";
            // var tokenResponse = await client.PostAsync(url, null);
            // var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(await tokenResponse.Content.ReadAsStringAsync());
            // var AccessToken = dict["access_token"].ToString();


            _pubsub.OnPubSubServiceConnected += (s, e) =>
            {
                _pubsub.ListenToFollows("428420296");
                //pubsub.ListenToSubscriptions("428420296");
                _pubsub.SendTopics(AccessToken);
            };

            _pubsub.OnFollow += (s, e) => messageSink.Send(new(
                new TwitchFollow(e.Username),
                "twitch-pubsub"
            ));

            _pubsub.OnChannelSubscription += (s, e) => messageSink.Send(new(
                new TwitchSubscription(
                    e.Subscription.UserId,
                    e.Subscription.Username,
                    e.Subscription.Time,
                    e.Subscription.TotalMonths,
                    e.Subscription.StreakMonths,
                    e.Subscription.Context
                ),
                "twitch-pubsub"
            ));

            _pubsub.Connect();

            return ValueTask.CompletedTask;
        }
    }
}