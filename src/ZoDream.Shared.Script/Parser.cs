using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace ZoDream.Shared.Script
{
    public class Parser
    {
        public Expression ParseProgram(TextReader reader)
        {
            return ParseChunk(new Lexer(reader));
        }

        public Expression ParseChunk(Lexer reader)
        {
            //var parameters = new List<ParameterExpression>();
            //var globalScope = new GlobalScope();

            //// Expression.Block()
            //return Expression.Lambda(globalScope.ExpressionBlock, parameters);
            return Expression.Empty();
        }
    }
}
