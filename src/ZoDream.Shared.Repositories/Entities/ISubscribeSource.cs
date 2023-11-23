using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Repositories.Entities
{
    public interface ISubscribeSource
    {
        public string Name { get; set; }

        public string GroupName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
