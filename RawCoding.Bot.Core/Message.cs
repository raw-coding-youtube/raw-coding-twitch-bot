using System;
using System.Threading.Tasks;

namespace MoistBot.Models
{
    public record Message
    {
        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;
        public static Message NoOp => new NoOp();
        public static ValueTask<Message> NoOpTask => ValueTask.FromResult((Message) new NoOp());

        public static ValueTask<Message> TaskFrom<T>(T msg)
            where T : Message
            => ValueTask.FromResult((Message) msg);
    }

    public record NoOp : Message
    {
    }
}