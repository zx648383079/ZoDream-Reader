using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("books")]
    [PrimaryKey("Id", AutoIncrement = false)]
    public class BookEntity : INovel, INovelSourceEntity
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Cover { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;
        public string Charset { get; set; } = string.Empty;

        public int Type { get; set; }

        public string LatestChapterTitle { get; set; } = string.Empty;
        public int LatestChapterAt { get; set; }
        public int LastCheckAt { get; set; }
        public int LastCheckCount { get; set; }
        public int ChapterCount { get; set; }

        public string CurrentChapterTitle { get; set; } = string.Empty;

        public int CurrentChapterAt { get; set; }
        public int CurrentChapterIndex { get; set; }
        public int CurrentChapterOffset { get; set; }

        public bool IsUpdateable { get; set; }

        public int Order { get; set; }
    }
}
