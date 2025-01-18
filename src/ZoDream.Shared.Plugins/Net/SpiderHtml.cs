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
            _node = BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(content)).GetAwaiter().GetResult();
        }

        public SpiderHtml(NetSpider spider, INode node)
        {
            Parent = this;
            _factory = spider;
            _node = node;
        }

        public SpiderHtml(NetSpider spider, INode node, string alias)
            : this (spider, node)
        {
            Alias = alias;
        }

        private readonly NetSpider _factory;
        private readonly INode _node;

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
            if (_node is not IParentNode o)
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
            if (_node is IElement ele)
            {
                return new SpiderText(_factory, ele.GetAttribute("href") ?? string.Empty, Alias);
            }
            return new SpiderText(_factory, string.Empty, Alias);
        }
        public ITextObject Text()
        {
            return new SpiderText(_factory, _node.TextContent, Alias);
        }

        public IBaseObject Clone()
        {
            return new SpiderHtml(_factory, _node, Alias);
        }

        public bool Empty()
        {
            return _node is null;
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
            return _node.TextContent;
        }
    }
}
