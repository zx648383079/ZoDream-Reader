using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {

        public Compiler Render(string code)
        {
            var globalScope = new GlobalScope(typeof(IGlobalFactory), typeof(IBaseObject));
            return Render(code, globalScope);
        }

        public Compiler Render(string code, GlobalScope scope)
        {
            var func = new Parser().ParseProgram(code, scope, [new KeyValuePair<string, Type>(GlobalScope.InstanceName, scope.InstanceType)]);
            return new Compiler(scope, func.Compile());
        }

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
