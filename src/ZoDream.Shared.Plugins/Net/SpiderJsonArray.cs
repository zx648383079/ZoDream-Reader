using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderJsonArray : IQueryableObject, IArrayObject
    {

        public SpiderJsonArray(NetSpider spider, IEnumerable<JsonElement> items)
        {
            Parent = this;
            _factory = spider;
            _items = items.ToArray();
        }

        private readonly NetSpider _factory;

        private readonly JsonElement[] _items;

        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }

        public int Count => _items.Count();

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
            return this;
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
            return new SpiderText(_factory, _items.First().GetProperty("href").GetString());
        }
        public ITextObject Text()
        {
            return new SpiderText(_factory, _items.First().GetString());
        }

        public IBaseObject Clone()
        {
            return this;
        }

        public bool Empty()
        {
            return !_items.Any();
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
            throw new NotImplementedException();
        }

        public IBaseObject First()
        {
            return Empty() ? _factory.Null(this) : new SpiderJson(_factory,
                _items.First());
        }

        public IBaseObject Last()
        {
            return Empty() ? _factory.Null(this) : new SpiderJson(_factory,
                _items.Last());
        }

        public IBaseObject Nth(int index)
        {
            return Count <= index ? _factory.Null(this) : 
                new SpiderJson(_factory,
                _items[index]);
        }

        public IEnumerator<IBaseObject> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return new SpiderJson(_factory, item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
