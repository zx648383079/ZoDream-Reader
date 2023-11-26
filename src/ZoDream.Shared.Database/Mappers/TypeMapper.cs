using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace ZoDream.Shared.Database.Mappers
{
    public class TypeMapper: IMapper
    {
        static readonly EnumMapper EnumMapper = new();
        public T? Map<T>(IDataReader reader)
            where T : class
        {
            var res = Map(reader, typeof(T));
            return res is null ? default : (T)res;
        }
        public object? Map(IDataReader reader, Type type, IDictionary<string, int> fieldMaps)
        {
            if (!type.IsClass)
            {
                return null;
            }
            var data = Activator.CreateInstance(type);
            foreach (var item in type.GetProperties())
            {
                if (!item.CanWrite)
                {
                    continue;
                }
                var name = ReflectionHelper.GetPropertyName(item);
                if (string.IsNullOrEmpty(name) || !fieldMaps.TryGetValue(name, out var i))
                {
                    continue;
                }
                item.SetValue(data, Map(reader, item.PropertyType, i));
            }
            return data;
        }

        public List<object> MapList(IDataReader reader, Type itemType)
        {
            var maps = GetFieldMap(reader);
            var items = new List<object>();
            while (reader.Read())
            {
                if (itemType.IsClass)
                {
                    items.Add(Map(reader, itemType, 0)!);
                    continue;
                }
                items.Add(Map(reader, itemType, maps)!);
            }
            return items;
        }


        public object? Map(IDataReader reader, Type type)
        {
            if (type.IsClass && type.GenericTypeArguments.Length == 0)
            {
                var maps = GetFieldMap(reader);
                return Map(reader, type, maps);
            }
            if (type.IsArray || type == typeof(IList<>) || type == typeof(IPage<>))
            {
                var items = MapList(reader, type.GetGenericArguments()[0]);
                if (type.IsArray)
                {
                    return items.ToArray();
                }
                if (type == typeof(IPage<>))
                {
                    var res = Activator.CreateInstance(type);
                    type.GetProperty("Items").SetValue(res, items);
                    return res;
                }
                return items;
            }
            if (type.IsClass)
            {
                return null;
            }
            return Map(reader, type, 0);
        }

        public T? Map<T>(IDataReader reader, int index)
        {
            var res = Map(reader, typeof(T), index);
            return res is null ? default : (T)res;
        }

        public object? Map(IDataReader reader, Type type, int index)
        {
            if (type.IsEnum)
            {
                return EnumMapper.FromString(type, reader.GetString(index));
            }
            if (type == typeof(byte))
            {
                return reader.GetByte(index);
            }
            if (type == typeof(short))
            {
                return reader.GetInt16(index);
            }
            if (type == typeof(char))
            {
                return reader.GetChar(index);
            }
            if (type == typeof(int))
            {
                return reader.GetInt32(index);
            }
            if (type == typeof(string))
            {
                return reader.GetString(index);
            }
            if (type == typeof(long))
            {
                return reader.GetInt64(index);
            }
            if (type == typeof(bool))
            {
                return reader.GetBoolean(index);
            }
            if (type == typeof(double))
            {
                return reader.GetDouble(index);
            }
            if (type == typeof(float))
            {
                return reader.GetFloat(index);
            }
            if (type == typeof(DateTime))
            {
                return reader.GetDateTime(index);
            }
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public T? Map<T>(IDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == name)
                {
                    return Map<T>(reader, i);
                }
            }
            return default;
        }

        private Dictionary<string, int> GetFieldMap(IDataReader reader)
        {
            var map = new Dictionary<string, int>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                map.Add(reader.GetName(i), i);
            }
            return map;
        }
    }
}
