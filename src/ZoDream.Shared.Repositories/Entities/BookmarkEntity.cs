using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("book_marks")]
    [PrimaryKey("Id")]
    public class BookmarkEntity: INovelMark
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public string BookName { get; set; } = string.Empty;
    }
}
