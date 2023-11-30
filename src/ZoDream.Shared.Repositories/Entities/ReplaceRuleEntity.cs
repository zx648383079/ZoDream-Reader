using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("replace_rules")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class ReplaceRuleEntity: IReplaceRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string MatchValue { get; set; } = string.Empty;
        public string ReplaceValue { get; set; } = string.Empty;

        public bool IsRegex { get; set; }
        public bool IsMatchTitle { get; set; }
        public bool IsMatchContent { get; set; } = true;

        public string IncludeMatch { get; set; } = string.Empty;
        public string ExcludeMatch { get; set; } = string.Empty;

        public int Timeout { get; set; } = 3000;

        public bool IsEnabled { get; set; } = true;
        public int SortOrder { get; set; } = 99;

    }
}
