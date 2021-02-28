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
    public class TwitchChatBot : IMessageSource
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

        public TwitchClient Client => _client;

        public ValueTask Register(IMessageContextSink messageContextSink)
        {
            _client.OnMessageReceived += (s, e) =>
            {
                var msg = e.ChatMessage;
                messageContextSink.Send(new(
                    new CustomerMessage(msg.Username, msg.Message),
                    msg.Channel,
                    Constants.Sources.TwitchChat
                ));
            };

            _client.OnJoinedChannel += (s, e) => messageContextSink.Send(new(
                new SendTwitchPublicMessage(e.Channel, "Moist Bot in the building, behave."),
                e.Channel,
                Constants.Sources.TwitchChat
            ));

            _client.Connect();
            return ValueTask.CompletedTask;
        }

        private static void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime}: {e.BotUsername} - {e.Data}");
        }
    }
}