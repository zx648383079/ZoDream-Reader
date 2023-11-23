using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Repositories.Entities
{
    public class SubscribeSourceEntity: ISubscribeSource
    {
        public string Name { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }
    }
}
