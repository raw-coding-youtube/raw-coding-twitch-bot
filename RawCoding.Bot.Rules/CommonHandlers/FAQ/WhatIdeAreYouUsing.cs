using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoistBot.Models;

namespace RawCoding.Bot.Rules.CommonHandlers.FAQ
{
    public class WhatIdeAreYouUsing : MessageHandler<CustomerMessage>
    {
        private static readonly Regex Pattern = new(@"\b(what|which)\b.+\b(ide|editor)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private const string Answer = "My main editors for C# are Rider https://www.jetbrains.com/rider/ and LinqPad https://www.linqpad.net/";

        protected override ValueTask<Message> Handle(CustomerMessage msg)
        {
            return Pattern.IsMatch(msg.Message)
                ? Message.TaskFrom(new QuestionAnswer(Answer))
                : Message.NoOpTask;
        }
    }
}