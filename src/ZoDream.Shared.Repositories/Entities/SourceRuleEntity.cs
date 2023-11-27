﻿using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("source_rules")]
    public class SourceRuleEntity: ISourceRule
    {
        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string BaseUri { get; set; } = string.Empty;

        public SourceType Type { get; set; }

        public string DetailUrlRule { get; set; } = string.Empty;

        public bool EnabledExplore { get; set; }

        public string ExploreUrl { get; set; } = string.Empty;
        public string ExploreMatchRule { get; set; } = string.Empty;
        public string SearchUrl { get; set; } = string.Empty;

        public string SearchMatchRule { get; set; } = string.Empty;

        public string DetailMatchRule { get; set; } = string.Empty;
        public string ContentMatchRule { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }
    }
}
