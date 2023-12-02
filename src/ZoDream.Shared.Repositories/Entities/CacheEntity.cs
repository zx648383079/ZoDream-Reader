using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("caches")]
    [PrimaryKey("Key", AutoIncrement = false)]
    public class CacheEntity: INovelCache
    {

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public int DeadAt { get; set; }
    }
}
