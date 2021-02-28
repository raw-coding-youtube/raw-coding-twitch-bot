using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoistBot.Models;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace RawCoding.Bot.Rules.Twitch.Sources
{
    [Lifetime(ServiceLifeTime.Singleton)]
    public class TwitchChatBot : IEventSource,
        ProcessEvent<SendTwitchPrivateMessage>,
        ProcessEvent<SendTwitchPublicMessage>
    {
        private readonly ILogger<TwitchChatBot> _logger;
        private static TwitchClient _client;

        public TwitchChatBot(
            IOptionsMonitor<TwitchSettings> optionsMonitor,
            ILogger<TwitchChatBot> logger
        )
        {
            _logger = logger;
            var credentials = new ConnectionCredentials("raw_coding", optionsMonitor.CurrentValue.AccessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);

            _client = new(customClient);
            _client.Initialize(credentials, "raw_coding");
            _client.OnLog += Client_OnLog;
        }

        public ValueTask Register(IEventSink eventSink)
        {
            _client.OnMessageReceived += (s, e) =>
            {
                var msg = e.ChatMessage;
                eventSink.Send(new ReceivedTwitchMessage(msg.Channel, msg.Username, msg.Message));
            };

            _client.OnJoinedChannel += (s, e) =>
            {
                eventSink.Send(new SendTwitchPublicMessage(e.Channel, "Moist Bot in the building, behave."));
            };

            _client.Connect();
            return ValueTask.CompletedTask;
        }

        private static void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }

        public Task Process(SendTwitchPrivateMessage e)
        {
            _client.SendWhisper(e.Username, e.Message);
            return Task.CompletedTask;
        }

        public Task Process(SendTwitchPublicMessage e)
        {
            _client.SendMessage(e.Channel, e.Message);
            return Task.CompletedTask;
        }
    }
}