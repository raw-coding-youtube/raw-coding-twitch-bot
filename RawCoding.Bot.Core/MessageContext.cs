namespace MoistBot.Models
{
    public record MessageContext(
        Message Message,
        string SourceId,
        string SourceType
    );
}