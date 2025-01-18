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
            _node = JsonDocument.Parse(content).RootElement;
        }

        public SpiderJson(NetSpider spider, JsonElement node)
        {
            Parent = this;
            _factory = spider;
            _node = node;
        }

        private readonly NetSpider _factory;
        private readonly JsonElement _node;
        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            var res = _factory.Array(this);
            res.Add(func.Invoke(this));
            return res;
        }
        public IQueryableObject Query(string selector)
        {
            throw new NotImplementedException();
        }
        public IBaseObject Attr(string name)
        {
            if (name.Equals(nameof(Text), StringComparison.CurrentCultureIgnoreCase))
            {
                return Text();
            }
            if (name.Equals(nameof(Href), StringComparison.CurrentCultureIgnoreCase))
            {
                return Href();
            }
            return _factory.Null(this);
        }
        public ITextObject Href()
        {
            return new SpiderText(_factory, string.Empty);
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
            return _node.GetPropertyCount() == 0;
        }

        public IBaseObject Is(IBaseObject condition, IBaseObject trueResult)
        {
            return Is(condition.Empty(), trueResult);
        }

        public IBaseObject Is(IBaseObject condition, IBaseObject trueResult, IBaseObject falseResult)
        {
            return Is(condition.Empty(), trueResult, falseResult);
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult)
        {
            return Is(condition, trueResult, this);
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult, IBaseObject falseResult)
        {
            return condition ? trueResult : falseResult;
        }

        public override string ToString()
        {
            return _node.GetString() ?? string.Empty;
        }
    }
}
