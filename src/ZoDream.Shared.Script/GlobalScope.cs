using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ZoDream.Shared.Script
{
    public class GlobalScope(Type instanceType, Type returnType): Scope(null)
    {
        public const string InstanceName = "#instance";
        public const string ReturnName = "#return";

        private readonly Dictionary<string, ParameterExpression> ParameterItems = [];
        public Type InstanceType { get; private set; } = instanceType;

        public override Type? ReturnType => returnType;

        public LabelTarget ReturnLabel { get; set; } = Expression.Label(returnType, ReturnName);

        public ParameterExpression RegisterParameter(Type type, string name)
                => ParameterItems[name] = Expression.Parameter(type, name);

        public override Expression? LookupExpression(string name, bool isLocalOnly = false)
        {
            if (ParameterItems.TryGetValue(name, out var p))
            {
                return p;
            }
            return base.LookupExpression(name, isLocalOnly);
        }

        public MethodInfo? GetMethod(string name, Type[] types)
        {
            var method = InstanceType.GetMethod(name, types);
            var upperName = Studly(name);
            if (method == null && name != upperName)
            {
                method = InstanceType.GetMethod(upperName, types);
            }
            return method;
        }

        public static string Studly(string val)
        {
            var data = val.Split('-', '_', ' ');
            var res = new StringBuilder();
            foreach (var item in data)
            {
                res.Append(item.Substring(0, 1).ToUpper());
                res.Append(item.Substring(1));
            }
            return res.ToString();
        }
    }
}
