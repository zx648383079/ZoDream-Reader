using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public abstract partial class SQLGrammar
    {

        public string Visit(Expression exp)
        {
            if (exp == null)
            {
                return string.Empty;
            }
            return exp.NodeType switch
            {
                ExpressionType.Lambda => VisitLambda(exp as LambdaExpression),
                ExpressionType.MemberAccess => VisitMemberAccess(exp as MemberExpression),
                ExpressionType.Constant => VisitConstant(exp as ConstantExpression),
                ExpressionType.Add or ExpressionType.AddChecked or ExpressionType.Subtract or ExpressionType.SubtractChecked or ExpressionType.Multiply or ExpressionType.MultiplyChecked or ExpressionType.Divide or ExpressionType.Modulo or ExpressionType.And or ExpressionType.AndAlso or ExpressionType.Or or ExpressionType.OrElse or ExpressionType.LessThan or ExpressionType.LessThanOrEqual or ExpressionType.GreaterThan or ExpressionType.GreaterThanOrEqual or ExpressionType.Equal or ExpressionType.NotEqual or ExpressionType.Coalesce or ExpressionType.ArrayIndex or ExpressionType.RightShift or ExpressionType.LeftShift or ExpressionType.ExclusiveOr => VisitBinary(exp as BinaryExpression),
                ExpressionType.Conditional => VisitConditional(exp as ConditionalExpression),
                ExpressionType.Negate or ExpressionType.NegateChecked or ExpressionType.Not or ExpressionType.Convert or ExpressionType.ConvertChecked or ExpressionType.ArrayLength or ExpressionType.Quote or ExpressionType.TypeAs => VisitUnary(exp as UnaryExpression),
                ExpressionType.Parameter => VisitParameter(exp as ParameterExpression),
                ExpressionType.Call => VisitMethodCall(exp as MethodCallExpression),
                ExpressionType.New => VisitNew(exp as NewExpression),
                ExpressionType.NewArrayInit or ExpressionType.NewArrayBounds => VisitNewArray(exp as NewArrayExpression),
                ExpressionType.MemberInit => this.VisitMemberInit((MemberInitExpression)exp),
                _ => exp.ToString(),
            };
        }

        private string VisitUnary(UnaryExpression? unaryExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitMemberInit(MemberInitExpression exp)
        {
            throw new NotImplementedException();
        }

        private string VisitNew(NewExpression? newExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitMethodCall(MethodCallExpression? methodCallExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitParameter(ParameterExpression? parameterExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitNewArray(NewArrayExpression? newArrayExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitBinary(BinaryExpression? binaryExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitConditional(ConditionalExpression? conditionalExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitConstant(ConstantExpression? constantExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitMemberAccess(MemberExpression? memberExpression)
        {
            throw new NotImplementedException();
        }

        private string VisitLambda(LambdaExpression? lambdaExpression)
        {
            throw new NotImplementedException();
        }
    }
}
