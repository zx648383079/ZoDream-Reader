using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ISpider: IDisposable
    {

        public ISpider From(string uri);

        public ISpider Header(IDictionary<string, object> data);
        public ISpider Header(string key, string value);
        public ISpider Post(IDictionary<string, object> data);
        public ISpider Html();
        public ISpider Json();
        public ISpider Xml();

        public ISpider Query(string query);

        public ISpider Text();

        public ISpider Split(string separator);
        public ISpider Split(string separator, int count);

        public ISpider Match(string pattern);
        public ISpider Match(string pattern, int tag);
        public ISpider Match(string pattern, string tag);

        public ISpider Map();
    }
}
