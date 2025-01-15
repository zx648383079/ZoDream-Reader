using System;
using System.Collections.Generic;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderMap : Dictionary<string, IBaseObject>, IMapObject
    {
        public IBaseObject As(string name)
        {
            throw new NotImplementedException();
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
    }
}
