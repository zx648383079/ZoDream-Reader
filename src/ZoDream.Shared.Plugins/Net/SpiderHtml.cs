using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
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

        private readonly NetSpider _factory;
        private readonly IDocument _doc;

        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            return _factory.Array(this);
        }

        public IQueryableObject Query(string selector)
        {
            _doc.QuerySelectorAll(selector);
            return this;
        }

        public IBaseObject Attr(string name)
        {
            return _factory.Null(this);
        }

        public ITextObject Href()
        {
            return new SpiderText(_factory, _doc.TextContent);
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
