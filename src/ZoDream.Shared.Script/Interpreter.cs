using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {
        public object Execute(string code, object target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(object));
            return Execute(code, globalScope, target);
        }

        public object Execute(string code, GlobalScope scope, object target)
        {
            var func = new Parser().ParseProgram(code, scope, [new KeyValuePair<string, Type>(GlobalScope.InstanceName, target.GetType())]);
            return func.Compile().DynamicInvoke(target);
        }

        public T Execute<T>(string code, object target)
        {
            var globalScope = new GlobalScope(target.GetType(), typeof(T));
            return (T)Execute(code, globalScope, target);
        }
    }
}
