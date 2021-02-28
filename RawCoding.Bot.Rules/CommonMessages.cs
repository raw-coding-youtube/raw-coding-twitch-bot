using MoistBot.Models;

namespace RawCoding.Bot.Rules
{
    public record CustomerMessage(string Author, string Message) : Message;
    public record QuestionAnswer(string Message) : Message;
    public record ExecuteCommand(string Author, string Message) : Message;
}