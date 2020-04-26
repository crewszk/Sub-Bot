namespace SClassBot
{
    public class Token
    {
        public string StandardPrefix { get; set; }
        public string ModPrefix { get; set; }
        public string BotToken { get; set; }
        public Channel GuildChannels { get; set; }
    }

    public class Channel
    {
        public ulong Welcome { get; set; }
        public ulong Rules { get; set; }
        public ulong Birthday { get; set; }
        public ulong SelfRole { get; set; }
        public ulong Goodbye { get; set; }
        public ulong BotLog { get; set; }
    }
}