using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IReadRecord
    {

        public string Name { get; set; }

        public long ReadTime { get; set; }

        public long LastReadAt { get; set; }
    }
}
