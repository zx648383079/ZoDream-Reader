using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("dictionary_rules")]
    public class DictionaryRuleEntity: IDictionaryRule
    {
        public string Name { get; set; } = string.Empty;

        public string UrlRule { get; set; } = string.Empty;
        public string ShowRule { get; set; } = string.Empty;


        public bool IsEnabled { get; set; } = true;

    }
}
