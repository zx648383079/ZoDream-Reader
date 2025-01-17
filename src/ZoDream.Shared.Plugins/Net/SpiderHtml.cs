using AngleSharp;
using AngleSharp.Dom;
using System;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderHtml : IQueryableObject
    {

        public SpiderHtml(NetSpider spider, string content)
        {
            Parent = this;
            _factory = spider;
            _doc = BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(content)).GetAwaiter().GetResult();
        }

        public SpiderHtml(NetSpider spider, INode node)
        {
            Parent = this;
            _factory = spider;
            _doc = node;
        }

        private readonly NetSpider _factory;
        private readonly INode _doc;

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
            if (_doc is not IParentNode o)
            {
                return new SpiderNull(_factory);
            }
            return new SpiderHtmlArray(_factory, o.QuerySelectorAll(selector));
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
            return new SpiderText(_factory, _doc.TextContent);
        }

        public IBaseObject Clone()
        {
            return this;
        }

        public bool Empty()
        {
            return _doc is null;
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
    }
}
