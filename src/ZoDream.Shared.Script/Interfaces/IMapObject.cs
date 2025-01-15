using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IMapObject : IDictionary<string, IBaseObject>, IBaseObject
    {

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func);


        public IBaseObject First();

        public IBaseObject Last();

        public IBaseObject Nth(int index);
    }
}
