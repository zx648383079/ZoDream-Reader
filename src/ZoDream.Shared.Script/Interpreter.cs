using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {
        public IBaseObject Execute(string code, IGlobalFactory target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(IBaseObject));
            return Execute(code, globalScope, target);
        }

        public IBaseObject Execute(string code, GlobalScope scope, IGlobalFactory target)
        {
            try
            {
                var func = new Parser().ParseProgram(code, scope, [new KeyValuePair<string, Type>(GlobalScope.InstanceName, target.GetType())]);
                return (IBaseObject)func.Compile().DynamicInvoke(target);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return target.Null(target);
            }
        }

        public T Execute<T>(string code, IGlobalFactory target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(T));
            return (T)Execute(code, globalScope, target);
        }
    }
}
