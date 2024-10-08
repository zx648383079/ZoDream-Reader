﻿using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IGobalFactory
    {
        public ITextObject Get(string url);
        public ITextObject Post(string url, IDictionary<string, object> body);
        public IUrlObject Url(string url);
    }
}
