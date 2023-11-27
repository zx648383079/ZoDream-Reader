namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ISubscribeSource
    {
        public string Name { get; set; }

        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
