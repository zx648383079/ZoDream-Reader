using System;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("book_chapters")]
    [PrimaryKey("Id")]
    public class ChapterEntity: INovelChapter
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;

        public long Begin { get; set; }

        public long End { get; set; }

        public int Index { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsLoaded { get; set; }

        public DateTime PublishedAt { get; }
    }
}
