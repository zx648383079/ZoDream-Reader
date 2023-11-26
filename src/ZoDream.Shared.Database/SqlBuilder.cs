using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    public class SqlBuilder<T>(IDatabase database): ISqlBuilder<T>
    {

        public IDatabase Database { get; private set; } = database;

        public ISqlBuilder<T> From()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> GroupBy()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> Limit()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> OrderBy()
        {
            throw new NotImplementedException();
        }

        public Page<T> Page()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> Select()
        {
            throw new NotImplementedException();
        }

        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        public List<T> ToList()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> Where()
        {
            throw new NotImplementedException();
        }
    }
}
