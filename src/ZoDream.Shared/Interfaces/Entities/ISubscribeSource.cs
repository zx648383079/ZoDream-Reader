namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ISubscribeSource : IRuleItem
    {
        public string GroupName { get; set; }

        public string Url { get; set; }

    }
}
