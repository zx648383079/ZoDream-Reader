using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IArrayObject : IEnumerable<IBaseObject>, IBaseObject
    {
        public int Count { get; }

        public void Add(IBaseObject item);

        public IArrayObject Map(Func<IBaseObject, IBaseObject> func);

        public IBaseObject First();

        public IBaseObject Last();

        public IBaseObject Nth(int index);
    }
}
