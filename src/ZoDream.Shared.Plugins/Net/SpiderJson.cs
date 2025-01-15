using Newtonsoft.Json;
using System;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderJson : IQueryableObject
    {

        public SpiderJson(NetSpider spider, string content)
        {
            _spider = spider;
            _doc = JsonConvert.DeserializeObject(content);
        }

        private readonly NetSpider _spider;
        private readonly object? _doc;

        public IQueryableObject Query(string selector)
        {
            throw new NotImplementedException();
        }

        public ITextObject Text()
        {
            return new SpiderText(_spider, string.Empty);
        }

        public object Clone()
        {
            return new SpiderJson(_spider, string.Empty);
        }

        public IBaseObject As(string name)
        {
            throw new NotImplementedException();
        }

        IBaseObject IBaseObject.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
