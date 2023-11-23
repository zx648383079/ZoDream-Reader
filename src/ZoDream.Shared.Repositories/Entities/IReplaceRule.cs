﻿using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;

namespace ZoDream.Shared.Repositories.Entities
{
    public interface IReplaceRule
    {
        public string Name { get; set; }

        public string GroupName { get; set; }

        public string MatchValue { get; set; }
        public string ReplaceValue { get; set; } 

        public bool IsRegex { get; set; }
        public bool IsMatchTitle { get; set; }
        public bool IsMatchContent { get; set; }

        public string IncludeMatch { get; set; }
        public string ExcludeMatch { get; set; }

        public int Timeout { get; set; }

        public bool IsEnabled { get; set; }

    }
}
