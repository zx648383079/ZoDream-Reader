namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IChapterRule: IRuleItem
    {

        public string MatchRule { get; set; }

        public string Example { get; set; }

    }
}
