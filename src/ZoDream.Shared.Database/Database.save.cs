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

        public int Delete(string tableName, string primaryKeyName, object data)
        {
            throw new NotImplementedException();
        }

        public int Delete(string tableName, string primaryKeyName, object? data, object? primaryKeyValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(object data)
        {
            throw new NotImplementedException();
        }

        public int Delete<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public int Delete<T>(object dataOrPrimaryKey)
        {
            throw new NotImplementedException();
        }

        public object Insert<T>(string tableName, string primaryKeyName, bool autoIncrement, T data)
        {
            throw new NotImplementedException();
        }

        public object Insert<T>(string tableName, string primaryKeyName, T data)
        {
            throw new NotImplementedException();
        }

        public object Insert<T>(T data)
        {
            throw new NotImplementedException();
        }

        public int Update(string tableName, string primaryKeyName, object data, object primaryKeyValue)
        {
            throw new NotImplementedException();
        }

        public int Update(string tableName, string primaryKeyName, object data)
        {
            throw new NotImplementedException();
        }

        public int Update(string tableName, string primaryKeyName, object data, object? primaryKeyValue, IEnumerable<string>? columns)
        {
            throw new NotImplementedException();
        }

        public int Update(string tableName, string primaryKeyName, object data, IEnumerable<string>? columns)
        {
            throw new NotImplementedException();
        }

        public int Update(object data, IEnumerable<string> columns)
        {
            throw new NotImplementedException();
        }

        public int Update(object data, object primaryKeyValue, IEnumerable<string>? columns)
        {
            throw new NotImplementedException();
        }

        public int Update(object data)
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T poco, Expression<Func<T, object>> fields)
        {
            throw new NotImplementedException();
        }

        public int Update(object data, object primaryKeyValue)
        {
            throw new NotImplementedException();
        }

        public int Update<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
