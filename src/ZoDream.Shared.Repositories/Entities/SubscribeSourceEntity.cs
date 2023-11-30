using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("rss_sources")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class SubscribeSourceEntity: ISubscribeSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; } = 99;
    }
}
