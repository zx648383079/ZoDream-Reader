using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtNovelChapter : INovelChapter
    {
        public TxtNovelChapter(string title, long begin, long end)
        {
            Title = title;
            Begin = begin;
            End = end;
        }

        public TxtNovelChapter(string title, long begin, long end, DateTime publishedAt)
            : this(title, begin, end)
        {
            PublishedAt = publishedAt;
        }
        public string Title { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public DateTime PublishedAt { get; private set; } = DateTime.MinValue;
        public long Begin { get; private set; }
        public long End { get; private set; }
    }
}
