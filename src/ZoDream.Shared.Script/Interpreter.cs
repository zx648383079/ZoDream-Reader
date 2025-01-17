using System;
using System.Collections.Generic;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {
        public IBaseObject Execute(string code, object target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(IBaseObject));
            return Execute(code, globalScope, target);
        }

        public IBaseObject Execute(string code, GlobalScope scope, object target)
        {
            var func = new Parser().ParseProgram(code, scope, [new KeyValuePair<string, Type>(GlobalScope.InstanceName, target.GetType())]);
            return (IBaseObject)func.Compile().DynamicInvoke(target);
        }

        public T Execute<T>(string code, object target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(T));
            return (T)Execute(code, globalScope, target);
        }
    }
}
