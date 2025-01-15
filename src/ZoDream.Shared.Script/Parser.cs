using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace ZoDream.Shared.Script
{
    public class Parser
    {

        public LambdaExpression ParseProgram(string content, GlobalScope globalScope, IEnumerable<KeyValuePair<string, Type>> args)
        {
            using var reader = new StringReader(content);
            return ParseProgram(reader, globalScope, args);
        }
        public LambdaExpression ParseProgram(TextReader reader, GlobalScope globalScope, IEnumerable<KeyValuePair<string, Type>> args)
        {
            return ParseChunk(new Lexer(reader), globalScope, args);
        }

        public LambdaExpression ParseChunk(Lexer reader, GlobalScope globalScope, IEnumerable<KeyValuePair<string, Type>> args)
        {
            var parameters = new List<ParameterExpression>();
            foreach (var arg in args)
            {
                parameters.Add(globalScope.RegisterParameter(arg.Value, arg.Key));
            }
            if (reader.CurrentToken is null)
            {
                reader.NextToken();
            }
            ParseBlock(globalScope, reader);
            return Expression.Lambda(globalScope.ExpressionBlock, parameters);
        }

        private void ParseBlock(GlobalScope globalScope, Lexer reader)
        {
            var inLoop = true;
            while (inLoop)
            {
                switch(reader.CurrentToken!.Type)
                {
                    case TokenType.Eof:
                        inLoop = false;
                        break;
                    case TokenType.Dot:
                        reader.NextToken();
                        break;
                    case TokenType.Bracket:
                        if (reader.CurrentToken.Value == "{")
                        {
                            ParseMap(globalScope, reader);
                        }
                        break;
                    case TokenType.Identifier:
                        var name = reader.CurrentToken.Value;
                        var token = reader.NextToken();
                        if (token.Type == TokenType.Colon)
                        {
                            // 命名
                            break;
                        }
                        if (token.Type == TokenType.Dot)
                        {
                            Expression.Call(
                            globalScope.LookupExpression(GlobalScope.InstanceName),
                            globalScope.GetMethod(name, []));
                            break;
                        }
                        if (token.Value == "(")
                        {
                            ParseParameter(globalScope, reader);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private Expression ParseMap(GlobalScope globalScope, Lexer reader)
        {
            var type = typeof(Dictionary<string, object>);
            var data = Expression.New(type.GetConstructor(BindingFlags.Public, null, [], []));
            var func = type.GetMethod("Add", [typeof(string), typeof(object)]);
            var key = string.Empty;
            var isKey = true;
            while (true)
            {
                var token = reader.NextToken();
                if (token.Type == TokenType.Eof)
                {
                    break;
                }
                if (token.Type == TokenType.Bracket && token.Value == "}")
                {
                    break;
                }
                if (isKey)
                {
                    if (token.Type == TokenType.Colon)
                    {
                        isKey = false;
                        continue;
                    }
                    key += token.Value;
                    continue;
                }
                if (token.Value == "{")
                {
                    ParseMap(globalScope, reader);
                }
                if (token.Type == TokenType.Comma)
                {

                    
                }
            }
            Expression.Call(data, func, Expression.Constant(key));
            return data;
        }

        private Expression ParseParameter(GlobalScope globalScope, Lexer reader)
        {
            switch (reader.CurrentToken?.Type)
            {
                case TokenType.String:
                    return Expression.Constant(reader.CurrentToken.Value);
                case TokenType.Number:
                    return Expression.Constant(Convert.ToInt32(reader.CurrentToken.Value));
                case TokenType.Comma:
                    break;
                case TokenType.Bracket:
                    break;
                default:
                    break;
            }
            return Expression.Empty();
        }
    }
}
