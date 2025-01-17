using System;
using System.Collections.Generic;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderArray : List<IBaseObject>, IArrayObject
    {
        public SpiderArray(NetSpider spider)
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

        public IBaseObject First()
        {
            return First();
        }

        public IBaseObject Last()
        {
            return Last();
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            var items = new SpiderArray(_factory);
            foreach (var item in this) 
            {
                items.Add(func.Invoke(item.Clone()));
            }
            return items;
        }

        public IBaseObject Nth(int index)
        {
            return this[index];
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
    }
}
