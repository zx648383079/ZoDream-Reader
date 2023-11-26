using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
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
            try
            {
                Open();
                using var cmd = CreateCommand(_sharedConnection, commandType, sql, args);
                var val = cmd.ExecuteScalar();
                if (val == null || val == DBNull.Value)
                {
                    return default!;
                }
                var t = typeof(T);
                var u = Nullable.GetUnderlyingType(t);
                return (T)Convert.ChangeType(val, u ?? t);
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
            return Build<T>().ToList();
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return (List<T>)(object)Fetch(typeof(T), sql, args);
        }

        public List<T> Fetch<T>(long page, long perPage, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IPage<T> Page<T>(long page, long perPage, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }


        public T First<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Dictionary<TKey, TValue> Pluck<TKey, TValue>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public List<T> Pluck<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
