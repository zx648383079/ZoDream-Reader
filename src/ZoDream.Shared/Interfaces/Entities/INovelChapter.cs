﻿using System;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovelChapter
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Url { get; set; }

        public string BookId { get; }

        public long Begin { get; }

        public long End { get; set; }

        public int Index { get; set; }

        public string Description { get; }

        public DateTime PublishedAt { get; }
    }
}
