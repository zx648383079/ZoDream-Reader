﻿using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("chapter_rules")]
    public class ChapterRuleEntity: IChapterRule
    {
        public string Name { get; set; } = string.Empty;

        public string MatchRule { get; set; } = string.Empty;

        public string Example { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }
    }
}
