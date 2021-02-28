using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace RawCoding.Bot.FAQ
{
    public class ChatMoisturizer
    {
        private readonly List<IQuestionable> _questions;
        private readonly TwitchClient _client;

        public ChatMoisturizer(TwitchClient client)
        {
            _client = client;
            var questions = typeof(ChatMoisturizer)
                            .Assembly
                            .ExportedTypes
                            .Where(x => !x.GetTypeInfo().IsInterface
                                        && x.GetTypeInfo().GetInterfaces().Contains(typeof(IQuestionable)))
                            .Select(q => (IQuestionable) Activator.CreateInstance(q));

            _questions = new List<IQuestionable>(questions);
        }

        public void Moisturize(OnMessageReceivedArgs e)
        {
            foreach (var q in _questions)
                if (q.Match(e.ChatMessage.Message))
                {
                    _client.SendMessage(e.ChatMessage.Channel, q.Answer);
                    return;
                }
        }
    }
}