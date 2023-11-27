using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    public class SourceChannelEntity: INovelChannel
    {

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}
