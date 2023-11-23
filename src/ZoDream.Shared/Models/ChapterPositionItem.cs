using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class ChapterPositionItem
    {
        public string Title { get; set; } = string.Empty;

        public long Position { get; set; }

        public ChapterPositionItem()
        {
            
        }

        public ChapterPositionItem(string title)
        {
            Title = title;
        }
    }
}
