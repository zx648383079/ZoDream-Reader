using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Script
{
    public class Token
    {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }


        public TokenType Type { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
            => string.Format("{0}='{1}'", Type, Value);
    }
}
