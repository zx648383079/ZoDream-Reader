namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovelCache
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public int DeadAt { get; set; }
    }
}
