using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("book_groups")]
    public class BookGroupEntity: INovelGroup
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
