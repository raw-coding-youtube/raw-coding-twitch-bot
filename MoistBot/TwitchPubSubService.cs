using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MoistBot.Infrastructure;
using MoistBot.Models;
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
        //428420296
        private readonly IHttpClientFactory _factory;
        private readonly IHubContext<TwitchHub> _twitchHub;
        private readonly IServiceProvider _provider;
        private readonly TwitchSettings _twitchSettings;
        private readonly TwitchPubSub _pubsub;

        public TwitchPubSubService(
            IHttpClientFactory factory,
            IOptionsMonitor<TwitchSettings> optionsMonitor,
            IHubContext<TwitchHub> twitchHub,
            IServiceProvider provider)
        {
            _factory = factory;
            _twitchHub = twitchHub;
            _provider = provider;
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
            using (var scope = _provider.CreateScope())
            {
                var userRegister = scope.ServiceProvider.GetRequiredService<RegisterUserAction>();

                var result = await userRegister.TrySaveFollow(e.UserId);

                if (result > 0)
                    await _twitchHub.Clients.All.SendAsync("follow", e.DisplayName);
            }
        }

        private async void UserSubscribed(object sender, OnChannelSubscriptionArgs e)
        {
            using (var scope = _provider.CreateScope())
            {
                var userRegister = scope.ServiceProvider.GetRequiredService<RegisterUserAction>();

                var subscription = new UserSubscription
                {
                    TwitchUserId = e.Subscription.UserId,
                    TwitchUsername = e.Subscription.Username,
                    Time = e.Subscription.Time,
                    SubscriptionPlan = (SubscriptionPlan) e.Subscription.SubscriptionPlan,
                    SubscriptionPlanName = e.Subscription.SubscriptionPlanName,
                    Months =  e.Subscription.Months ?? 0,
                    Context = e.Subscription.Context,
                 };

                await userRegister.SaveSubscriber(subscription);

                await _twitchHub.Clients.All.SendAsync("follow", e.Subscription.Username);
            }
        }
    }
}