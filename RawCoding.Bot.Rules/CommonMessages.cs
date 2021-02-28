using MoistBot.Models;

namespace RawCoding.Bot.Rules
{
    public record CustomerMessage(string Author, string Message) : Message;
    public record Respond(string Customer, string Message, bool Private = false) : Message;
    public record ExecuteCommand(string Author, string Message) : Message;
    public record Noop() : ExecuteCommand("", "");
}