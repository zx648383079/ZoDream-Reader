using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Repositories.Entities
{
    public class ChapterEntity
    {

        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;

        public long Begin { get; set; }

        public long End { get; set; }
    }
}
