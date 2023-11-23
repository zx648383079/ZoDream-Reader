using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Script
{
    public partial class Lexer
    {
        public Lexer(TextReader reader)
        {
            Reader = reader;
        }


        protected Token ReadToken()
        {
            return new Token(TokenType.None, string.Empty);
        }
    }
}
