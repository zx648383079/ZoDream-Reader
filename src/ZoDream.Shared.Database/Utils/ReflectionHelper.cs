using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZoDream.Shared.Database
{
    internal static class ReflectionHelper
    {
        public static string GetPropertyName(PropertyInfo info)
        {
            foreach (var item in info.GetCustomAttributes())
            {
                if (item is IgnoreAttribute)
                {
                    return string.Empty;
                }
                if (item is ColumnAttribute column)
                {
                    return string.IsNullOrWhiteSpace(column.Name) ? info.Name : column.Name;
                }
            }
            return info.Name;
        }

        public static string GetTableName(Type info)
        {
            foreach (var item in info.GetCustomAttributes())
            {
                if (item is TableNameAttribute table)
                {
                    return table.Value;
                }
            }
            return info.Name;
        }
    }
}
