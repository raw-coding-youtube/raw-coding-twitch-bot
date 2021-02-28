using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoistBot.Models;
using RawCoding.Bot.Rules.Twitch;

namespace RawCoding.Bot.Rules.CommonHandlers
{
    public class IdentifyQuestions : MessageHandler<CustomerMessage>
    {
        protected override async ValueTask Handle(CustomerMessage msg)
        {
            var (author, message) = msg;
            foreach (var questionsAndAnswer in _questionsAndAnswers)
            {
                if (questionsAndAnswer.QuestionPattern.IsMatch(message))
                {
                    await Broadcast(new Respond(author, message));
                }
            }
        }

        private static readonly ReadOnlyCollection<QuestionAnswer> _questionsAndAnswers = new List<QuestionAnswer>
        {
            new()
            {
                QuestionPattern = new(@"\b(what|which)\b.+\b(ide|editor)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                Answer = "My main editors for C# are Rider https://www.jetbrains.com/rider/ and LinqPad https://www.linqpad.net/"
            }
        }.AsReadOnly();

        private struct QuestionAnswer
        {
            public Regex QuestionPattern { get; set; }
            public string Answer { get; set; }
        }
    }
}