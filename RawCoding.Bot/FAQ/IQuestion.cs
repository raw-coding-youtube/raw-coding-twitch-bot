namespace RawCoding.Bot.FAQ
{
    public interface IQuestionable
    {
        bool Match(string message);
        string Answer { get; }
    }
}