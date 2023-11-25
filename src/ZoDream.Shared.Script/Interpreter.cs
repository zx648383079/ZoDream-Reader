using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {
        public object Execute(string code)
        {
            var func = new Parser().ParseProgram(code);
            return func.Compile().DynamicInvoke();
        }

        public T Execute<T>(string code)
        {
            return default;
        }
    }
}
