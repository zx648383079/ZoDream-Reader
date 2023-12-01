using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    public interface ISqlBuilder<T> 
        where T : class
    {
        public ISqlBuilder<T> From<K>() where K : class;
        public ISqlBuilder<T> From(string tableName);
        public ISqlBuilder<T> IsEmpty();

        public ISqlBuilder<T> When(bool condition, Action<SqlBuilder<T>> trueFunc);
        public ISqlBuilder<T> When(bool condition, Action<SqlBuilder<T>> trueFunc, Action<SqlBuilder<T>> falseFunc);
        public ISqlBuilder<T> Where(string key, object val);
        public ISqlBuilder<T> Where(string key, string @operator, object val);

        public ISqlBuilder<T> OrWhere(string key, object val);
        public ISqlBuilder<T> OrWhere(string key, string @operator, object val);

        public ISqlBuilder<T> OrWhereIn(string key, params object[] val);
        public ISqlBuilder<T> WhereIn(string key, params object[] val);
        public ISqlBuilder<T> OrWhereNotIn(string key, params object[] val);
        public ISqlBuilder<T> WhereNotIn(string key, params object[] val);
        public ISqlBuilder<T> WhereNull(string key);
        public ISqlBuilder<T> WhereNotNull(string key);
        public ISqlBuilder<T> WhereLike(string key, string val);
        public ISqlBuilder<T> OrWhereNull(string key);
        public ISqlBuilder<T> OrWhereNotNull(string key);
        public ISqlBuilder<T> OrWhereLike(string key, string val);

        public ISqlBuilder<T> WhereBetween(string key, object begin, object end);
        public ISqlBuilder<T> OrWhereBetween(string key, object begin, object end);

        public ISqlBuilder<T> Having(string key, object val);
        public ISqlBuilder<T> Having(string key, string @operator, object val);

        public ISqlBuilder<T> OrHaving(string key, object val);
        public ISqlBuilder<T> OrHaving(string key, string @operator, object val);
        public ISqlBuilder<T> SelectCall(string func, string key, string alias);
        public ISqlBuilder<T> Select(params string[] items);
        public ISqlBuilder<T> Limit(int size);
        public ISqlBuilder<T> Limit(int begin, int size);
        public ISqlBuilder<T> Skip(int amount);

        public ISqlBuilder<T> Take(int count);
        public ISqlBuilder<T> OrderBy(string key, bool desc = false);
        public ISqlBuilder<T> OrderByDesc(string key);
        public ISqlBuilder<T> OrderByAsc(string key);
        public ISqlBuilder<T> GroupBy(string key);

        public K Max<K>(string key);
        public K Min<K>(string key);
        public double Average(string key);
        public K Sum<K>(string key);
        public bool Any();
        public int Count();
        public int Count(string key);

        public K? Scalar<K>();
        public K? Value<K>(string key);
        public long LongCount();
        public T? First();
        public T FirstOrDefault();
        public List<T> ToList();
        public T[] ToArray();
        public Page<T> Page(long page, long perPage);

        public int Delete();
        public int Update(T data);
        public int Update(string key, object val);
        public int UpdateBool(string key);
        public int UpdateIncrement(string key, int offset = 1);
        public int UpdateIncrement(string key, float offset);
        public int UpdateIncrement(string key, double offset);
        public int UpdateDecrement(string key, int offset = 1);
        public int UpdateDecrement(string key, float offset);
        public int UpdateDecrement(string key, double offset);
        public int Update(IDictionary<string, object> data);
        public object? Insert(IDictionary<string, object> data);
        public object? Insert(T data);
        public int Insert(IEnumerable<T> items);

    }
}
