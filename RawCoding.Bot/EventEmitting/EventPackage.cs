namespace RawCoding.Bot.EventEmitting
{
    public class EventPackage
    {
        public string Target { get; set; }
        public object Attributes { get; set; }
        public int DisplayTime { get; set; }
    }

    public struct Targets
    {
        public const string Follow = nameof(Follow);
        public const string Subscribe = nameof(Subscribe);
    }
}