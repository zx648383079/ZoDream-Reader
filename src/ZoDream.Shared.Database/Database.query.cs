using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace ZoDream.Shared.Database
{
    public partial class Database
    {

        public int Execute(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public List<object> Fetch(Type type, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public List<T> Fetch<T>()
        {
            throw new NotImplementedException();
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
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
