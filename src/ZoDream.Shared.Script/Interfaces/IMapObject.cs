using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IMapObject : IDictionary<string, IBaseObject>, IArrayObject
    {
    }
}
