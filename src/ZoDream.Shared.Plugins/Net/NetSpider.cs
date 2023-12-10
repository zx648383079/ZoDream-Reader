using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class NetSpider : ISpider
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ISpider From(string uri)
        {
            throw new NotImplementedException();
        }

        public ISpider Header(IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public ISpider Header(string key, string value)
        {
            throw new NotImplementedException();
        }

        public ISpider Html()
        {
            throw new NotImplementedException();
        }

        public ISpider Json()
        {
            throw new NotImplementedException();
        }

        public ISpider Map()
        {
            throw new NotImplementedException();
        }

        public ISpider Match(string pattern)
        {
            throw new NotImplementedException();
        }

        public ISpider Match(string pattern, int tag)
        {
            throw new NotImplementedException();
        }

        public ISpider Match(string pattern, string tag)
        {
            throw new NotImplementedException();
        }

        public ISpider Post(IDictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public ISpider Query(string query)
        {
            throw new NotImplementedException();
        }

        public ISpider Split(string separator)
        {
            throw new NotImplementedException();
        }

        public ISpider Split(string separator, int count)
        {
            throw new NotImplementedException();
        }

        public ISpider Text()
        {
            throw new NotImplementedException();
        }

        public ISpider Xml()
        {
            throw new NotImplementedException();
        }
    }
}
