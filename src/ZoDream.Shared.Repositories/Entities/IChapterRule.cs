using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;

namespace ZoDream.Shared.Repositories.Entities
{
    public interface IChapterRule
    {
        public string Name { get; set; }

        public string MatchRule { get; set; }

        public string Example { get; set; }

        public bool IsEnabled { get; set; }

    }
}
