using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers.FAQ
{
    public class WhatIdeAreYouUsing : MessageHandler<CustomerMessage>
    {
        private readonly IMessageSink _sink;

        private static readonly Regex Pattern =
            new Regex(@"\b(what|which)\b.+\b(ide|editor)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private const string Answer = "My main editors for C# are Rider https://www.jetbrains.com/rider/ and LinqPad https://www.linqpad.net/";

        public WhatIdeAreYouUsing(IMessageSink sink) => _sink = sink;

        protected override ValueTask Handle(CustomerMessage msg)
        {
            return Pattern.IsMatch(msg.Message)
                ? _sink.Send(new QuestionAnswer(msg.Message))
                : ValueTask.CompletedTask;
        }
    }
}