using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Script
{
    public class Interpreter
    {
        public object Execute(string code)
        {
            return false;
        }

        public T Execute<T>(string code)
        {
            return default;
        }
    }
}
