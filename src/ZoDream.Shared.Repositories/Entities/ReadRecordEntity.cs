using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("read_records")]
    [PrimaryKey("Name")]
    public class ReadRecordEntity: IReadRecord
    {

        public string Name { get; set; } = string.Empty;

        public long ReadTime { get; set; }

        public long LastReadAt { get; set; }
    }
}
