using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ZoDream.Shared.Script
{
    public class Scope(Scope? parent)
    {

        private readonly Scope? Parent = parent;

        private readonly List<Expression> BlockItems = [];

        private Dictionary<string, Expression>? VariableItems = null;

        private bool IsBlockGenerated;

        public Type? ExpressionBlockType { get; set; }
        public bool ExistExpressions => BlockItems.Count > 0;

        public virtual Type? ReturnType => Parent?.ReturnType;
        public ParameterExpression[] Variables => VariableItems == null ? [] : (from v in VariableItems.Values where v is ParameterExpression select (ParameterExpression)v).ToArray();

        public virtual bool ActivateRethrow => Parent != null && Parent.ActivateRethrow;

        private void CheckBlockGenerated()
        {
            if (IsBlockGenerated)
            {
                throw new InvalidOperationException();
            }
        }

        public void AddExpression(Expression expr)
        {
            CheckBlockGenerated();
            BlockItems.Add(expr);
        }

        public ParameterExpression RegisterVariable(Type type, string sName)
                => RegisterVariable(Expression.Variable(type, sName));
        public ParameterExpression RegisterVariable(ParameterExpression expr)
        {
            VariableItems ??= [];
            VariableItems[expr.Name] = expr;
            return expr;
        }

        public virtual Expression? LookupExpression(string name, bool isLocalOnly = false)
        {
            if (VariableItems != null && VariableItems.TryGetValue(name, out var p))
            {
                return p;
            }

            return Parent != null && !isLocalOnly ?
                Parent.LookupExpression(name) :
                null;
        }

        public virtual Expression ExpressionBlock {
            get {
                CheckBlockGenerated();
                IsBlockGenerated = true;
                if (BlockItems.Count == 0)
                {
                    return Expression.Empty();
                }
                else
                {
                    var variables = Variables;
                    if (variables.Length == 0)
                    {
                        return Expression.Block(BlockItems);
                    }
                    else if (ExpressionBlockType == null)
                    {
                        return Expression.Block(variables, BlockItems);
                    }
                    else
                    {
                        return Expression.Block(ExpressionBlockType, variables, BlockItems);
                    }
                }
            }
        }
    }
}
