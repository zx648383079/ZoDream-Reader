using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ZoDream.Shared.Database
{
    public interface IDatabaseQuery
    {

        public int Execute(string sql, params object[] args);

        public T ExecuteScalar<T>(string sql, params object[] args);

        public List<object> Fetch(Type type, string sql, params object[] args);

        public List<T> Fetch<T>();
        public List<T> Fetch<T>(string sql, params object[] args);

        public List<T> Fetch<T>(long page, long perPage, string sql, params object[] args);

        public IPage<T> Page<T>(long page, long perPage, string sql, params object[] args);

        public T First<T>(string sql, params object[] args);

        public T FirstOrDefault<T>(string sql, params object[] args);

        public Dictionary<TKey, TValue> Pluck<TKey, TValue>(string sql, params object[] args);

        public List<T> Pluck<T>(string sql, params object[] args);

        public IQuery<T> Query<T>();
    }
}
