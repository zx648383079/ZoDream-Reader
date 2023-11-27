namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IChapterRule
    {
        public string Name { get; set; }

        public string MatchRule { get; set; }

        public string Example { get; set; }

        public bool IsEnabled { get; set; }

    }
}
