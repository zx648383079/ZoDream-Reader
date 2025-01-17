using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Script
{
    public class Compiler
    {

        internal Compiler(GlobalScope scope, Delegate chunk)
        {
            Scope = scope;
            Chunk = chunk;
        }

        public Delegate Chunk { get; private set; }
        public GlobalScope Scope { get; private set; }


        public IBaseObject Execute(IGlobalFactory factory)
        {
            var res = Chunk.DynamicInvoke(factory);
            if (res is null)
            {
                return factory.Null(factory);
            }
            return (IBaseObject)res;
        }

        public T? Execute<T>(IGlobalFactory factory)
        {
            var res = Chunk.DynamicInvoke(factory);
            if (res is null)
            {
                return default;
            }
            return (T)res;
        }
    }
}
