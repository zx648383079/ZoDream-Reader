using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ZoDream.Shared.Database
{
    public partial class Database
    {

        public int Delete(string tableName, string primaryKeyName, object data)
        {
            return Execute(Grammar.CompileDelete(tableName, primaryKeyName), data);
        }



        public int Delete<T>(string sql, params object[] args)
             where T : class
        {
            return Execute(Grammar.CompileDeleteJoin(ReflectionHelper.GetTableName(typeof(T)), sql), args);
        }

        public int Delete<T>(object dataOrPrimaryKey)
             where T : class
        {
            var type = typeof(T);
            return Delete(ReflectionHelper.GetTableName(type), ReflectionHelper.GetPrimaryKey(type), dataOrPrimaryKey);
        }

        public object? Insert<T>(
            string tableName, 
            string primaryKeyName, 
            bool autoIncrement, T data) where T : class
        {
            var type = typeof(T);
            var items = new List<object>();
            var keys = new List<string>();
            var primaryKeyType = typeof(object);
            foreach (var item in type.GetProperties())
            {
                var name = ReflectionHelper.GetPropertyName(item);
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                var val = item.GetValue(data);
                if (name == primaryKeyName)
                {
                    primaryKeyType = item.PropertyType;
                    if (autoIncrement && ReflectionHelper.IsEmpty(val, item.PropertyType))
                    {
                        continue;
                    }
                }
                keys.Add(name);
                items.Add(val);
            }
            var i = keys.IndexOf(primaryKeyName);
            var sql = Grammar.CompileInsert(tableName, primaryKeyName, keys);
            if (i == -1)
            {
                return ExecuteScalar(primaryKeyType, sql, CommandType.Text, items);
            }
            var res = Execute(Grammar.CompileInsert(tableName, primaryKeyName, keys), items);
            if (res > 0)
            {
                return items[i];
            }
            return null;
        }

        public object? Insert<T>(string tableName, string primaryKeyName, T data) where T : class
        {
            return Insert(tableName, primaryKeyName, true, data);
        }

        public object? Insert<T>(T data) where T : class
        {
            var type = typeof(T);
            var attr = type.GetCustomAttribute<PrimaryKeyAttribute>();
            if (attr is not null)
            {
                return Insert(ReflectionHelper.GetTableName(type), attr.Value, 
                    attr.AutoIncrement, data);
            }
            var key = ReflectionHelper.GetPrimaryKey(type);
            return Insert(ReflectionHelper.GetTableName(type), key, data);
        }

        public int Update(string tableName, string primaryKeyName, object primaryKeyValue, object data, IEnumerable<string>? columns = null)
        {
            var type = data.GetType();
            var items = new List<object>();
            if (columns is not null)
            {
                foreach (var key in columns)
                {
                    items.Add(ReflectionHelper.GetPropertyValue(key, type, data));
                }
                return Execute(Grammar.CompileUpdate(tableName, primaryKeyName, columns), primaryKeyValue, items);
            }
            var keys = new List<string>();
            foreach (var item in type.GetProperties())
            {
                var name = ReflectionHelper.GetPropertyName(item);
                if (string.IsNullOrEmpty(name) || name == primaryKeyName)
                {
                    continue;
                }
                keys.Add(name);
                items.Add(item.GetValue(data));
            }
            return Execute(Grammar.CompileUpdate(tableName, primaryKeyName, keys), primaryKeyValue, items);
        }

        public int Update(string tableName, string primaryKeyName, object data, IEnumerable<string>? columns = null)
        {
            var type = data.GetType();
            return Update(tableName, primaryKeyName,
                ReflectionHelper.GetPropertyValue(primaryKeyName, type, data), data, columns);
        }

        public int Update<T>(T data, IEnumerable<string> columns) where T : class
        {
            var type = typeof(T);
            var key = ReflectionHelper.GetPrimaryKey(type);
            return Update(ReflectionHelper.GetTableName(type), key,
                ReflectionHelper.GetPropertyValue(key, type, data), data, columns);
        }

        public int Update<T>(T data, object primaryKeyValue, IEnumerable<string>? columns) where T : class
        {
            var type = typeof(T);
            var key = ReflectionHelper.GetPrimaryKey(type);
            return Update(ReflectionHelper.GetTableName(type), key, primaryKeyValue, data, columns);
        }

        public int Update<T>(T data) where T : class
        {
            var type = typeof(T);
            var key = ReflectionHelper.GetPrimaryKey(type);
            return Update(ReflectionHelper.GetTableName(type), key, 
                ReflectionHelper.GetPropertyValue(key, type, data), data);
        }

        public int Update<T>(string sql, params object[] args)
             where T : class
        {
            return Execute(Grammar.CompileUpdateJoin(ReflectionHelper.GetTableName(typeof(T)), sql), args);
        }
    }
}
