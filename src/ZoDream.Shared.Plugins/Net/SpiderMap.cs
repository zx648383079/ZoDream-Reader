using System;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderMap : Dictionary<string, IBaseObject>, IMapObject
    {
        public SpiderMap(NetSpider spider)
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
            return new SpiderMap(_factory);
        }

        public IBaseObject First()
        {
            return Empty() ? _factory.Null(this) : Values.First();
        }

        public IBaseObject Last()
        {
            return Empty() ? _factory.Null(this) : Values.Last();
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            return _factory.ToArray(Values.Select(func));
        }

        public IBaseObject Nth(int index)
        {
            return Count > index ? Values.Skip(index).Take(1).First() : _factory.Null(this);
        }

        public bool Empty()
        {
            return Count == 0;
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
            if (ContainsKey(item.Alias))
            {
                this[item.Alias] = item;
                return;
            }
            Add(item.Alias, item);
        }

        IEnumerator<IBaseObject> IEnumerable<IBaseObject>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
