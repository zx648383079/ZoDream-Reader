using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ISearchHistory
    {
        public string Word { get; set; }

        public int UseCount { get; set; }

        public long LastUseAt { get; set; }
    }
}
