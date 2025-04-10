﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderNull : INullObject, ITextObject, IQueryableObject, 
        IUrlObject, IArrayObject
    {
        public SpiderNull(NetSpider spider)
        {
            _factory = spider;
            Parent = this;
        }

        private readonly NetSpider _factory;
        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }

        public int Count => 0;

        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IBaseObject Clone()
        {
            return this;
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            return _factory.Array(this);
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

        public IBaseObject Attr(string name)
        {
            return this;
        }
        public ITextObject Href()
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
            return this;
        }

        public IArrayObject Split(string tag, int count)
        {
            return this;
        }

        public IArrayObject Match(string pattern)
        {
            return this;
        }

        public ITextObject Match(string pattern, int group)
        {
            return this;
        }



        public ITextObject Match(string pattern, string group)
        {
            return this;
        }

        public IArrayObject Match(Regex pattern)
        {
            return this;
        }
        public ITextObject Match(Regex pattern, int group)
        {
            return this;
        }
        public ITextObject Match(Regex pattern, string group)
        {
            return this;
        }

        public ITextObject Replace(string pattern, string replacement)
        {
            return this;
        }
        public ITextObject Replace(Regex pattern, string replacement)
        {
            return this;
        }

        public ITextObject Append(string text)
        {
            return this;
        }
        public ITextObject Append(IBaseObject text)
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

        public void Add(IBaseObject item)
        {
            
        }

        public IEnumerator<IBaseObject> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
