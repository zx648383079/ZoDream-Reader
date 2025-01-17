using System;
using System.Collections.Generic;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderNull : INullObject, ITextObject, IQueryableObject, 
        IUrlObject
    {
        public SpiderNull(NetSpider spider)
        {
            _factory = spider;
            Parent = this;
        }

        private readonly NetSpider _factory;
        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IBaseObject Clone()
        {
            return this;
        }

        public IBaseObject Map(Func<IBaseObject, IBaseObject> func)
        {
            return this;
        }

        public IBaseObject First()
        {
            return this;
        }

        public IBaseObject Last()
        {
            return this;
        }

        public IBaseObject Nth(int index)
        {
            return this;
        }

        public IQueryableObject Query(string selector)
        {
            return this;
        }

        public ITextObject Text()
        {
            return this;
        }

        public IUrlObject Method(string method)
        {
            return this;
        }

        public IUrlObject Query(string key, string value)
        {
            return this;
        }

        public IUrlObject Query(IDictionary<string, object> queries)
        {
            return this;
        }

        public IUrlObject Proxy(string proxy)
        {
            return this;
        }

        public IUrlObject Header(string key, string value)
        {
            return this;
        }

        public IUrlObject Header(IDictionary<string, object> queries)
        {
            return this;
        }

        public ITextObject Get()
        {
            return this;
        }

        public ITextObject Post(IDictionary<string, object> body)
        {
            return this;
        }

        public ITextObject Execute()
        {
            return this;
        }

        public IQueryableObject Html()
        {
            return this;
        }

        public IQueryableObject Xml()
        {
            return this;
        }

        public IUrlObject Url()
        {
            return this;
        }

        public IQueryableObject Json()
        {
            return this;
        }

        public IArrayObject Split(string tag)
        {
            return new SpiderArray(_factory);
        }

        public IArrayObject Split(string tag, int count)
        {
            return new SpiderArray(_factory);
        }

        public IArrayObject Match(string pattern)
        {
            return new SpiderArray(_factory);
        }

        public ITextObject Match(string pattern, int group)
        {
            return this;
        }

        public ITextObject Match(string pattern, string group)
        {
            return this;
        }

        public ITextObject Get(string url)
        {
            return this;
        }

        public ITextObject Post(string url, IDictionary<string, object> body)
        {
            return this;
        }

        public IUrlObject Url(string url)
        {
            return this;
        }

        public bool Eq(string text)
        {
            return false;
        }

        public bool Empty()
        {
            return true;
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
