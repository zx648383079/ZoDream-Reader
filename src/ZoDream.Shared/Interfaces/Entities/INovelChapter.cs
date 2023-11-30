using System;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovelChapter
    {
        public int Id { get; set; }
        public string Title { get; }

        public string Url { get; }

        public string BookId { get; }

        public long Begin { get; }

        public long End { get; }

        public string Description { get; }

        public DateTime PublishedAt { get; }
    }
}
