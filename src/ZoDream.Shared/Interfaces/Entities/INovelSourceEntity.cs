namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovelSourceEntity
    {
        public int Type { get; }

        public string FileName { get; }

        public string Charset { get; }
    }
}
