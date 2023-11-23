using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Repositories.Entities
{
    public class CacheEntity
    {

        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public int DeadAt { get; set; }
    }
}
