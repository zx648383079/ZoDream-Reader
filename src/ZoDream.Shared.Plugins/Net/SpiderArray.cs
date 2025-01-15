using System;
using System.Collections.Generic;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderArray : List<IBaseObject>, IArrayObject
    {
        public IBaseObject As(string name)
        {
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
            var items = new SpiderArray();
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


        
    }
}
