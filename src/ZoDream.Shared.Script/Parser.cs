using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;

namespace ZoDream.Shared.Script
{
    public class Parser
    {

        public LambdaExpression ParseProgram(string content)
        {
            using var reader = new StringReader(content);
            return ParseProgram(reader);
        }
        public LambdaExpression ParseProgram(TextReader reader)
        {
            return ParseChunk(new Lexer(reader));
        }

        public LambdaExpression ParseChunk(Lexer reader)
        {
            while (true)
            {
                var token = reader.NextToken();
                if (token.Type == TokenType.Eof)
                {
                    break;
                }
                Debug.WriteLine(token.ToString());
                // Expression.Constant(token.Value);
                //var call = Expression.New(typeof());
                //Expression.Call(call, typeof().GetMethod(token.Value));
            }
            //var parameters = new List<ParameterExpression>();
            //var globalScope = new GlobalScope();

            var block = Expression.Block();
            return Expression.Lambda(block, []);
            // return Expression.Empty();
        }

    }
}
