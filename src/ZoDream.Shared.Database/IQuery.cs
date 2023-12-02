using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ZoDream.Shared.Database
{
    public interface IQuery<T>
    {

        public IQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);

        public IQuery<T> Where(Expression<Func<T, bool>> predicate);
        public IQuery<T> OrderBy<K>(Expression<Func<T, K>> keySelector);
        public IQuery<T> OrderByDesc<K>(Expression<Func<T, K>> keySelector);
        public IQuery<T> Skip(int count);
        public IQuery<T> Take(int count);

        public T First();
        public T First(Expression<Func<T, bool>> predicate);

        public T FirstOrDefault();

        public T FirstOrDefault(Expression<Func<T, bool>> predicate);

        public List<T> ToList();

        public bool Any();

        public int Count();

        public long LongCount();

        public TResult Max<TResult>(Expression<Func<T, TResult>> selector);

        public TResult Min<TResult>(Expression<Func<T, TResult>> selector);

        public int? Sum(Expression<Func<T, int?>> selector);

        public long? Sum(Expression<Func<T, long?>> selector);

        public double? Average(Expression<Func<T, int>> selector);
    }
}
