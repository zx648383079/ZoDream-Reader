using System;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderObjectCollection<T> : List<T>, IObjectCollection<T>
    {
        public T? First()
        {
            return this.First<T>();
        }

        public T? Last()
        {
            return this.Last<T>();
        }

        public IObjectCollection<object> Map(Func<T, object> func)
        {
            var items = new SpiderObjectCollection<object>();
            foreach (var item in this) 
            {
                items.Add(func?.Invoke(item is ICloneable c ? (T)c.Clone() : item));
            }
            return items;
        }

        public T? Nth(int index)
        {
            return this[index];
        }


        
    }
}
