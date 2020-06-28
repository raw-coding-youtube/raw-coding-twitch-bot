namespace MoistBot.FAQ
{
    public interface IQuestionable
    {
        bool Match(string message);
        string Answer { get; }
    }
}