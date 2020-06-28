using System.Text.RegularExpressions;

namespace MoistBot.FAQ.Questions
{
    public class WhatIde : IQuestionable
    {
        private static readonly Regex Pattern =
            new Regex(@"\b(what|which)\b.+\b(ide|editor)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool Match(string message)
        {
            return Pattern.IsMatch(message);
        }

        public string Answer { get; } = "This is Rider! Get it here: https://www.jetbrains.com/rider/";
    }
}