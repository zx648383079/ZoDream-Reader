using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using ZoDream.Shared.Script.Interfaces;

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
            var res = ParseBlock(globalScope, globalScope.LookupExpression(GlobalScope.InstanceName)!, reader);
            globalScope.AddExpression(Expression.Return(globalScope.ReturnLabel, res));
        }

        private Expression ParseBlock(Scope scope, Expression instance, Lexer reader)
        {
            var inLoop = true;
            var objType = instance.Type;
            Expression res = Expression.Empty();
            while (inLoop)
            {
                switch (reader.CurrentToken!.Type)
                {
                    case TokenType.Eof:
                        inLoop = false;
                        break;
                    case TokenType.Dot:
                        reader.NextToken();
                        break;
                    case TokenType.DotDot:
                        scope.AddExpression(
                            res = Expression.Field(instance, objType.GetField("Parent"))
                        );
                        reader.NextToken();
                        break;
                    case TokenType.Bracket:
                        if (reader.CurrentToken.Value == "{")
                        {
                            ParseMap(scope, instance, reader);
                        }
                        break;
                    case TokenType.Identifier:
                        var name = reader.CurrentToken.Value;
                        var token = reader.NextToken();
                        if (token.Type == TokenType.Colon)
                        {
                            // 命名
                            scope.AddExpression(
                                res = Expression.Call(
                                    instance,
                                    objType.GetMethod("As", [typeof(string)]),
                                    Expression.Constant(name)
                                )
                            );
                            break;
                        }
                        if (token.Type == TokenType.Dot)
                        {
                            scope.AddExpression(
                                res = Expression.Call(
                                        instance,
                                        objType.GetMethod(name, [])
                                )
                            );
                            break;
                        }
                        if (token.Value == "(")
                        {
                            reader.NextToken();
                            scope.AddExpression(
                                res = ParseCall(scope, instance, name, reader)
                                
                            );
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            return res;
        }
        private Expression ParseCall(Scope scope, 
            Expression instance, 
            string name, 
            Lexer reader)
        {
            var items = new List<Expression>();
            while (reader.CurrentToken!.Value != ")")
            {
                var next = ParseParameter(scope, instance, reader);
                if (next is DefaultExpression)
                {
                    continue;
                }
                items.Add(next);
            }
            var func = instance.Type.GetMethod(GlobalScope.Studly(name), items.Select(i => i.Type).ToArray());
            return Expression.Call(
                    instance,
                    func,
                    items);
        }
        private Expression ParseMap(Scope scope, Expression instance, Lexer reader)
        {
            var type = typeof(Dictionary<string, object>);
            var data = Expression.New(type.GetConstructor(BindingFlags.Public, null, [], []));
            scope.AddExpression(data);
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
                    ParseMap(scope, instance, reader);
                }
                if (token.Type == TokenType.Comma)
                {
                    isKey = true;
                    continue;
                }

                scope.AddExpression(Expression.Call(
                    data, 
                    func, 
                        Expression.Constant(key),
                        ParseParameter(scope, instance, reader)
                    ));
            }
            
            return data;
        }

        private Expression ParseParameter(Scope scope, Expression instance, Lexer reader)
        {
            var current = reader.CurrentToken!;
            if (current.Value is "," or ")")
            {
                return Expression.Empty();
            }
            reader.NextToken();
            switch (current.Type)
            {
                case TokenType.String:
                    return Expression.Constant(current.Value);
                case TokenType.Number:
                    return Expression.Constant(Convert.ToInt32(current.Value));
                case TokenType.Comma:
                    break;
                case TokenType.Regex:
                    return Expression.New(typeof(Regex).GetConstructor([typeof(string)]), Expression.Constant(current.Value));
                case TokenType.Identifier:
                    if (current.Value is "true" or "false")
                    {
                        return Expression.Constant(
                            current.Value is "true"
                        );
                    }
                    if (reader.CurrentToken.Type == TokenType.Colon)
                    {
                        var arg = Expression.Variable(typeof(IBaseObject));
                        scope.RegisterVariable(arg);
                        scope.AddExpression(Expression.Assign(arg, Expression.Call(instance, instance.Type.GetMethod("Clone"))));
                        ParseBlock(scope, arg, reader);
                        scope.AddExpression(
                            Expression.Assign(instance, Expression.Call(
                                arg,
                                arg.Type.GetMethod("As", [typeof(string)]),
                                Expression.Constant(current.Value)
                            ))
                        );
                        break;
                    }
                    break;
                case TokenType.Bracket:
                    if (current.Value == ")")
                    {
                        break;
                    }
                    if (current.Value is "{")
                    {
                        return ParseMap(scope, instance, reader);
                    }
                    break;
                case TokenType.Dot:
                case TokenType.DotDot:
                    {
                        var arg = Expression.Variable(typeof(IBaseObject));
                        scope.RegisterVariable(arg);
                        scope.AddExpression(Expression.Assign(arg, Expression.Call(instance, instance.Type.GetMethod("Clone"))));
                        ParseBlock(scope, arg, reader);
                    }
                    break;
                default:
                    break;
            }
            return Expression.Empty();
        }
    }
}
