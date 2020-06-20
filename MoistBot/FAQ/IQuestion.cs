namespace TwitchBot.FAQ
{
    public interface IQuestionable
    {
        bool Match(string message);
        string Answer { get; }
    }
}