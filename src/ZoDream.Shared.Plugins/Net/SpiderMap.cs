using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public IBaseObject First()
        {
            throw new NotImplementedException();
        }

        public IBaseObject Last()
        {
            throw new NotImplementedException();
        }

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func)
        {
            throw new NotImplementedException();
        }

        public IBaseObject Nth(int index)
        {
            throw new NotImplementedException();
        }

        public bool Empty()
        {
            return Count == 0;
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
