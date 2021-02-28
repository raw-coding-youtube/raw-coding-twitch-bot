using System;

namespace MoistBot.Models
{
    public record MessageContext(
        Message Message,
        string Source
    );
}