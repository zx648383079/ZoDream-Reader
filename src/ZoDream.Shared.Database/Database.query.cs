﻿using System;
using System.Collections.Generic;
using System.Data;
using ZoDream.Shared.Database.Mappers;

namespace ZoDream.Shared.Database
{
    public partial class Database
    {

        public int Execute(string sql, params object[] args)
        {
            return Execute(sql, CommandType.Text, args);
        }

        public int Execute(string sql, CommandType commandType, params object[] args)
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, commandType, sql, args);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return ExecuteScalar<T>(sql, CommandType.Text, args);
        }

        public T ExecuteScalar<T>(string sql, CommandType commandType, params object[] args)
        {
            return (T)ExecuteScalar(typeof(T), sql, commandType, args);
        }

        public object ExecuteScalar(Type type, string sql, CommandType commandType, params object[] args)
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, commandType, sql, args);
                var val = cmd.ExecuteScalar();
                if (val == null || val == DBNull.Value)
                {
                    return default!;
                }
                var u = Nullable.GetUnderlyingType(type);
                return Convert.ChangeType(val, u ?? type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<object> Fetch(Type type, string sql, params object[] args)
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, CommandType.Text, sql, args);
                var reader = cmd.ExecuteReader();
                return new TypeMapper().MapList(reader, type)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<T> Fetch<T>()
        {
            return Fetch<T>(Grammar.CompileSelect(ReflectionHelper.GetTableName(typeof(T))));
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return (List<T>)(object)Fetch(typeof(T), sql, args);
        }

        public List<T> Fetch<T>(long page, long perPage, string sql, params object[] args)
        {
            return Fetch<T>(Grammar.CompileSelectJoin(
                ReflectionHelper.GetTableName(typeof(T)),
                sql,
                page, perPage
                ), args);
        }

        public IPage<T> Page<T>(long page, long perPage, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }


        public T? First<T>(string sql, params object[] args)
            where T : class
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, CommandType.Text, sql, args);
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return default;
                }
                return new TypeMapper().Map<T>(reader);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public T FirstOrDefault<T>(string sql, params object[] args) 
            where T : class
        {
            var res = First<T>(sql, args);
            if (res is null)
            {
                return Activator.CreateInstance<T>();
            }
            return res;
        }

        public Dictionary<TKey, TValue> Pluck<TKey, TValue>(string sql, params object[] args)
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, CommandType.Text, sql, args);
                var reader = cmd.ExecuteReader();
                return (Dictionary<TKey, TValue>)new TypeMapper().Map(reader, typeof(Dictionary<TKey, TValue>))!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<T> Pluck<T>(string sql, params object[] args)
        {
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, CommandType.Text, sql, args);
                var reader = cmd.ExecuteReader();
                return (List<T>)new TypeMapper().Map(reader, typeof(List<T>))!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}