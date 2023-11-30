using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("dictionary_rules")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class DictionaryRuleEntity: IDictionaryRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string UrlRule { get; set; } = string.Empty;
        public string ShowRule { get; set; } = string.Empty;


        public bool IsEnabled { get; set; } = true;
        public int SortOrder { get; set; } = 99;

    }
}
