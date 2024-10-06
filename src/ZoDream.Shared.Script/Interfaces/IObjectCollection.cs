using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IObjectCollection<T>: IList<T>
    {

        public IObjectCollection<object> Map(Func<T, object> func);


        public T? First();

        public T? Last();

        public T? Nth(int index);
    }
}
