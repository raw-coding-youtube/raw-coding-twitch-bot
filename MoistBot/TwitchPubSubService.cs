using System;
using System.Threading.Channels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoistBot.EventEmitting;
using MoistBot.Infrastructure;
using MoistBot.Models.Twitch;
using MoistBot.Models.Twitch.Enums;
using MoistBot.Services;
using TwitchLib.Api;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace MoistBot
{
    public class TwitchPubSubService
    {
        private readonly IServiceProvider _provider;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TwitchPubSubService> _logger;
        private readonly TwitchSettings _twitchSettings;
        private readonly TwitchPubSub _pubsub;
        private readonly ChannelWriter<EventPackage> _eventChannel;

        public TwitchPubSubService(
            IOptionsMonitor<TwitchSettings> optionsMonitor,
            Channel<EventPackage> eventChannel,
            IServiceProvider provider,
            IWebHostEnvironment env,
            ILogger<TwitchPubSubService> logger)
        {
            _eventChannel = eventChannel.Writer;
            _provider = provider;
            _env = env;
            _logger = logger;
            _twitchSettings = optionsMonitor.CurrentValue;
            _pubsub = new TwitchPubSub();

            _pubsub.OnPubSubServiceConnected += Pubsub_OnPubSubServiceConnected;
            _pubsub.OnFollow += ActionFollow;
            _pubsub.OnChannelSubscription += UserSubscribed;
        }

        private string AccessToken { get; set; }

        public void Start()
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

            _pubsub.Connect();
        }

        private void Pubsub_OnPubSubServiceConnected(object sender, System.EventArgs e)
        {
            _pubsub.ListenToFollows("428420296");
            //pubsub.ListenToSubscriptions("428420296");
            _pubsub.SendTopics(AccessToken);
        }

        private async void ActionFollow(object sender, OnFollowArgs e)
        {
            try
            {
                using (var scope = _provider.CreateScope())
                {
                    var userRegister = scope.ServiceProvider.GetRequiredService<RegisterUserAction>();

                    var result = await userRegister.TrySaveFollow(e.UserId);

                    if (_env.IsProduction() && result <= 0) return;

                    await _eventChannel.WriteAsync(new EventPackage
                    {
                        Target = Targets.Follow,
                        Attributes = new
                        {
                            e.DisplayName
                        },
                        DisplayTime = 6000
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error during: {0}", nameof(ActionFollow));
            }
        }

        private async void UserSubscribed(object sender, OnChannelSubscriptionArgs e)
        {
            using (var scope = _provider.CreateScope())
            {
                var userRegister = scope.ServiceProvider.GetRequiredService<RegisterUserAction>();

                var subscription = new TwitchSubscription
                {
                    UserId = e.Subscription.UserId,
                    TwitchUsername = e.Subscription.Username,
                    Time = e.Subscription.Time,
                    SubscriptionPlan = (SubscriptionPlan) e.Subscription.SubscriptionPlan,
                    SubscriptionPlanName = e.Subscription.SubscriptionPlanName,
                    TotalMonths = e.Subscription.TotalMonths,
                    StreakMonths = e.Subscription.StreakMonths,
                    Context = e.Subscription.Context,
                };

                await userRegister.SaveSubscription(subscription);

                await _eventChannel.WriteAsync(new EventPackage
                {
                    Target = Targets.Subscribe,
                    Attributes = new
                    {
                        subscription.TwitchUsername,
                        subscription.StreakMonths,
                        subscription.TotalMonths,
                    },
                    DisplayTime = 8000
                });
            }
        }
    }
}