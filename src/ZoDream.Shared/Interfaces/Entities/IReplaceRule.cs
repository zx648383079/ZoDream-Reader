namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IReplaceRule : IRuleItem
    {
        public string GroupName { get; set; }

        public string MatchValue { get; set; }
        public string ReplaceValue { get; set; } 

        public bool IsRegex { get; set; }
        public bool IsMatchTitle { get; set; }
        public bool IsMatchContent { get; set; }

        public string IncludeMatch { get; set; }
        public string ExcludeMatch { get; set; }

        public int Timeout { get; set; }


    }
}
