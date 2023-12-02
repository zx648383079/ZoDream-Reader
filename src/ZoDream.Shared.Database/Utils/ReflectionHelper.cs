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

        public static object GetPropertyValue(PropertyInfo info, object data)
        {
            return info.GetValue(data);
        }

        public static object GetPropertyValue(string name, Type type, object data)
        {
            var info = type.GetProperty(name);
            if (info is not null)
            {
                return GetPropertyValue(info, data);
            }
            foreach (var item in type.GetProperties())
            {
                if (!item.CanRead)
                {
                    continue;
                }
                foreach (var attr in item.GetCustomAttributes<ColumnAttribute>())
                {
                    if (attr.Name == name)
                    {
                        return GetPropertyValue(item, data);
                    }
                }
            }
            return data;
        }

        public static PropertyInfo? GetPropertyInfo(string name, Type type)
        {
            var info = type.GetProperty(name);
            if (info is not null)
            {
                return info;
            }
            foreach (var item in type.GetProperties())
            {
                foreach (var attr in item.GetCustomAttributes<ColumnAttribute>())
                {
                    if (attr.Name == name)
                    {
                        return item;
                    }
                }
            }
            return null;
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

        public static string GetPrimaryKey(Type info)
        {
            var attr = info.GetCustomAttribute<PrimaryKeyAttribute>();
            if (attr is not null)
            {
                return attr.Value;
            }
            foreach (var item in info.GetProperties())
            {
                var name = GetPropertyName(item);
                if (name.Equals("ID", StringComparison.CurrentCultureIgnoreCase))
                {
                    return name;
                }
            }
            return "ID";
        }

        public static (string[], PropertyInfo[]) GetProperties(Type info, bool isUpdated = false)
        {
            var fields = new List<string>();
            var atts = new List<PropertyInfo>();
            var attr = info.GetCustomAttribute<PrimaryKeyAttribute>();
            var key = string.Empty;
            if (attr is not null)
            {
                if (isUpdated)
                {
                    key = attr.Value;
                } else if (attr.AutoIncrement)
                {
                    key = attr.Value;
                }
            }
            foreach (var item in info.GetProperties())
            {
                var name = GetPropertyName(item);
                if (string.IsNullOrEmpty(name) || key == name || key == item.Name)
                {
                    continue;
                }
                fields.Add(name);
                atts.Add(item);
            }
            return (fields.ToArray(), atts.ToArray());
        }

        public static bool IsEmpty(object val, Type type)
        {
            if (val is null)
            {
                return true;
            }
            if (val is string s)
            {
                return string.IsNullOrWhiteSpace(s);
            }
            if (val is int i)
            {
                return i == 0;
            }
            if (val is long l)
            {
                return l == 0L;
            }
            if (val is float f)
            {
                return f == 0f;
            }
            if (val is double d)
            {
                return d == 0d;
            }
            return false;
        }
    }
}
