namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IDictionaryRule
    {
        public string Name { get; set; }

        public string UrlRule { get; set; }
        public string ShowRule { get; set; }


        public bool IsEnabled { get; set; }

    }
}
