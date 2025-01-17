using System;
using System.Text.Json;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderJson : IQueryableObject
    {

        public SpiderJson(NetSpider spider, string content)
        {
            Parent = this;
            _factory = spider;
            _doc = JsonDocument.Parse(content);
        }

        private readonly NetSpider _factory;
        private readonly JsonDocument _doc;
        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IQueryableObject Query(string selector)
        {
            throw new NotImplementedException();
        }

        public ITextObject Text()
        {
            return new SpiderText(_factory, string.Empty);
        }

        public IBaseObject Clone()
        {
            return new SpiderJson(_factory, string.Empty);
        }

        public bool Empty()
        {
            return _doc.RootElement.GetPropertyCount() == 0;
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult)
        {
            throw new NotImplementedException();
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult, IBaseObject falseResult)
        {
            throw new NotImplementedException();
        }
    }
}
