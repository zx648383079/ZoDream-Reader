using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface ISiteCookie
    {
        public string Url { get; set; }

        public string Cookie { get; set; }
    }
}
