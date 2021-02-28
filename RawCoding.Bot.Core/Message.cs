using System;

namespace MoistBot.Models
{
    public record Message
    {
        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;
    }
}