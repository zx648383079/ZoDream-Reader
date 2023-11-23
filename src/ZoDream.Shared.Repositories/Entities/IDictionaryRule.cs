using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;

namespace ZoDream.Shared.Repositories.Entities
{
    public interface IDictionaryRule
    {
        public string Name { get; set; }

        public string UrlRule { get; set; }
        public string ShowRule { get; set; }


        public bool IsEnabled { get; set; }

    }
}
