using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("search_histories")]
    public class SearchHistoryEntity: ISearchHistory
    {
        
        public string Word { get; set; } = string.Empty;

        public int UseCount { get; set; } = 1;

        public long LastUseAt { get; set; }
    }
}
