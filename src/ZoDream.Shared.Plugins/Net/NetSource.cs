using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Plugins.Net
{
    public class NetSource(string url): INovelSource
    {
        public NetSource(INovelSourceEntity entity)
            : this (entity.FileName)
        {
            
        }
        public string Url => url;
    }
}
