namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Cover { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string FileName { get; set; }
    }
}
