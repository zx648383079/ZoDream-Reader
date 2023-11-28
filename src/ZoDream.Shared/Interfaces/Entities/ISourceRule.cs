namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ISourceRule : IRuleItem
    {
        public string GroupName { get; set; }

        public string BaseUri { get; set; }

        public SourceType Type { get; set; }

        public string DetailUrlRule { get; set; }

        public bool EnabledExplore { get; set; }

        public string ExploreUrl { get; set; }
        public string ExploreMatchRule { get; set; }
        public string SearchUrl { get; set; }

        public string SearchMatchRule { get; set; }

        public string DetailMatchRule { get; set; }
        public string ContentMatchRule { get; set; }

        public string LoginUrl { get; set; }
        public IFormInput[] LoginForm { get; set; }

    }

    public enum SourceType
    {
        Text,
        Audio,
        Image,
        Video,
        File
    }
}
