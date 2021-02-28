using System;

namespace MoistBot.Models
{
    public record Event
    {
        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;
    }
}