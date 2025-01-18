using System;
using System.Collections.Generic;
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
        private int _index = 0;

        public string TemplateName => $"temp_{++_index}";

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
            var res = ParseBlock(globalScope, GetEntryInstance(globalScope), reader);
            globalScope.AddExpression(res);//Expression.Return(globalScope.ReturnLabel, res));
        }

        private Expression ParseBlock(Scope scope, Expression instance, Lexer reader)
        {
            var inLoop = true;
            Expression res = instance;
            while (inLoop)
            {
                switch (reader.CurrentToken!.Type)
                {
                    case TokenType.Eof:
                        inLoop = false;
                        break;
                    case TokenType.Comma:
                        inLoop = false;
                        reader.NextToken();
                        break;
                    case TokenType.Dot:
                        reader.NextToken();
                        break;
                    case TokenType.DotDot:
                        res = ParseParent(res);
                        reader.NextToken();
                        break;
                    case TokenType.Bracket:
                        if (reader.CurrentToken.Value == "{")
                        {
                            reader.NextToken();
                            ParseMap(scope, res, reader);
                            break;
                        }
                        if (reader.CurrentToken.Value == ")")
                        {
                            inLoop = false;
                            reader.NextToken();
                            break;
                        }
                        reader.NextToken();
                        break;
                    case TokenType.Identifier:
                        var name = reader.CurrentToken.Value;
                        var token = reader.NextToken();
                        if (token.Type == TokenType.Colon)
                        {
                            // 命名
                            res = Expression.Call(
                                    res,
                                    typeof(IBaseObject).GetMethod("As", [typeof(string)]),
                                    Expression.Constant(name)
                            );
                            break;
                        }
                        if (token.Value != "(")
                        {
                            res = ParseCall(
                                scope,
                                res,
                                name
                            );
                        }
                        else
                        {
                            reader.NextToken();
                            res = name.Equals("map", StringComparison.CurrentCultureIgnoreCase) ?
                                ParseConvertMap(scope, res, reader)
                                : ParseCall(scope, res, name, reader);
                            if (reader.CurrentToken.Value == ")")
                            {
                                reader.NextToken();
                            }
                        }
                        break;
                    default:
                        reader.NextToken();
                        break;
                }
            }
            return res;
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
                    if (reader.CurrentToken!.Type == TokenType.Colon)
                    {
                        reader.NextToken();
                        if (reader.CurrentToken.Type 
                            == TokenType.Dot)
                        {
                            reader.NextToken();
                            return ParseBlock(scope, ParseClone(scope, instance, current.Value), reader);
                        } else if (reader.CurrentToken.Type
                            == TokenType.DotDot)
                        {
                            reader.NextToken();
                            return ParseBlock(scope, ParseClone(scope, ParseParent(instance), current.Value), reader);
                        }
                        return ParseBlock(scope, ParseClone(scope, GetEntryInstance(scope), current.Value), reader);
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
                        return ParseBlock(scope, ParseClone(scope, instance), reader);
                    }
                default:
                    break;
            }
            return Expression.Empty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private Expression ParseConvertMap(Scope scope, Expression instance, Lexer reader)
        {
            var source = ParseVariable(scope, instance);
            var fn = ParseConvertMapFunc(scope, instance, reader);
            var res = Expression.Variable(typeof(IArrayObject), TemplateName);
            scope.RegisterVariable(res);
            scope.AddExpression(Expression.IfThenElse(Expression.TypeIs(source, typeof(IArrayObject)),
                    Expression.Assign(res,
                        Expression.Call(source, source.Type.GetMethod("Map", [fn.Type]),
                            fn
                        )
                    )
                    ,
                    Expression.Assign(res, 
                        Expression.Convert(
                            Expression.Call(
                        fn,
                        fn.Type.GetMethod("Invoke", [typeof(IBaseObject)])
                        , source
                        ),
                            res.Type
                            )
                    )
                ));
            return res;
        }

        private LambdaExpression ParseConvertMapFunc(Scope scope, Expression instance, Lexer reader)
        {
            var fnParameter = Expression.Parameter(typeof(IBaseObject));
            var block = new Scope(scope);
            var core = GetEntryInstance(scope);
            var host = Expression.Variable(typeof(IArrayObject), TemplateName);
            block.RegisterVariable(host);
            block.AddExpression(Expression.Assign(host, Expression.Call(
                    core,
                    core?.Type.GetMethod("Array", [typeof(IBaseObject)]),
                    fnParameter)));
            var func = host.Type.GetMethod("Add", [typeof(IBaseObject)]);
            while (reader.CurrentToken!.Value != ")" && reader.CurrentToken.Type != TokenType.Eof)
            {
                var next = ParseParameter(block, fnParameter, reader);
                if (next is DefaultExpression)
                {
                    continue;
                }
                block.AddExpression(Expression.Call(host, func, next));
            }
            var res = ParseClone(block, fnParameter);
            if (reader.CurrentToken.Type == TokenType.Eof)
            {
                block.AddExpression(Expression.Call(host, func, res));
            }
            else
            {
                reader.NextToken();
                block.AddExpression(Expression.Call(host, func, ParseBlock(block, res, reader)));
            }
            block.AddExpression(host);
            // block.AddExpression(Expression.Return(Expression.Label(), host, typeof(IBaseObject)));
            return Expression.Lambda<Func<IBaseObject, IBaseObject>>(block.ExpressionBlock, fnParameter);
        }

        private Expression ParseCall(Scope scope,
            Expression instance,
            string name,
            Lexer reader)
        {
            var items = new List<Expression>();
            while (reader.CurrentToken!.Value != ")" && reader.CurrentToken.Type != TokenType.Eof)
            {
                var next = ParseParameter(scope, instance, reader);
                if (next is DefaultExpression)
                {
                    continue;
                }
                items.Add(next);
            }
            if (reader.CurrentToken.Value != ")")
            {
                reader.NextToken();
            }
            return ParseCall(
                scope,
                    instance,
                    name,
                    [.. items]);
        }

        private Expression ParseCall(Scope scope, Expression instance,
            string name,
            params Expression[] parameters)
        {
            var func = instance.Type.GetMethod(GlobalScope.Studly(name), parameters.Select(i => i.Type).ToArray());
            if (func is not null)
            {
                return Expression.Call(
                    instance,
                    func,
                    parameters);
            }
            if (parameters.Length == 0 && !name.Equals("attr", StringComparison.CurrentCultureIgnoreCase)
                && !name.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                return ParseCall(scope, Expression.Convert(instance, typeof(IQueryableObject)), "attr", Expression.Constant(name));
            }
            var core = GetEntryInstance(scope);
            return Expression.Call(
                    core,
                    core?.Type.GetMethod("Null", [typeof(IBaseObject)]),
                    instance);
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


        private static Expression GetEntryInstance(Scope scope)
        {
            return scope.LookupExpression(GlobalScope.InstanceName)!;
        }

        private Expression ParseClone(Scope scope, Expression instance)
        {
            var res = ParseVariable(scope, instance);
            var next = Expression.Call(res, typeof(IBaseObject).GetMethod("Clone"));
            return ParseVariable(scope, Expression.TypeAs(next, instance.Type));
        }

        private Expression ParseClone(Scope scope, Expression instance, string alias)
        {
            var res = ParseVariable(scope, instance);
            var next = Expression.Call(res, typeof(IBaseObject).GetMethod("Clone")); 
            next = Expression.Call(
                next,
                typeof(IBaseObject).GetMethod("As", [typeof(string)]),
                Expression.Constant(alias)
            );
            return ParseVariable(scope, Expression.TypeAs(next, instance.Type));
        }

        private static MemberExpression ParseParent(Expression res)
        {
            return Expression.Field(res, res.Type.GetField("Parent"));
        }

        private Expression ParseVariable(Scope scope, Expression instance)
        {
            if (instance is ParameterExpression)
            {
                return instance;
            }
            var res = Expression.Variable(instance.Type, TemplateName);
            scope.RegisterVariable(res);
            scope.AddExpression(Expression.Assign(res, instance));
            return res;
        }
    }
}
