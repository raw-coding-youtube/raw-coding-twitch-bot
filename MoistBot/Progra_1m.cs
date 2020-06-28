using System;
using System.Threading;
using System.Threading.Tasks;
using MoistBot.FAQ;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace MoistBot.Zoink
{
    public static class Program
    {
        private static ChatMoisturizer _moisturizer;
        private static TwitchClient _client;
        private static readonly SemaphoreSlim Flag = new SemaphoreSlim(0);

        public static Task Mainaa()
        {
            var credentials = new ConnectionCredentials("raw_coding", "");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);

            _client = new TwitchClient(customClient);
            _client.Initialize(credentials, "raw_coding");

            _client.OnLog += Client_OnLog;
            _client.OnJoinedChannel += Client_OnJoinedChannel;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnWhisperReceived += Client_OnWhisperReceived;
            _client.OnNewSubscriber += Client_OnNewSubscriber;
            _client.Connect();

            _moisturizer = new ChatMoisturizer(_client);
            return Flag.WaitAsync();
        }

        private static void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private static void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _client.SendMessage(e.Channel, "Moist Bot in the building, behave.");
        }


        private static void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith('!'))
            {
                switch (e.ChatMessage.Message)
                {
                    case "!help":
                        _client.SendWhisper(e.ChatMessage.Username, "!ide - what ide is this?");
                        break;
                }
            }

            _moisturizer.Moisturize(e);
        }

        private static void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
                _client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private static void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                _client.SendMessage(e.Channel,
                                    $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                _client.SendMessage(e.Channel,
                                    $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
    }
}