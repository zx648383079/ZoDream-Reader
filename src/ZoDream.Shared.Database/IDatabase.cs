using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZoDream.Shared.Database.Adapters;

namespace ZoDream.Shared.Database
{
    public interface IDatabase: IDatabaseQuery, IDisposable
    {

        public IBuilderGrammar Grammar { get; }

        public object? Insert<T>(string tableName, string primaryKeyName, 
            bool autoIncrement, T data) where T : class;
        public object? Insert<T>(string tableName, string primaryKeyName, T data) where T : class;

        public object? Insert<T>(T data) where T : class;
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Insert<T>(IEnumerable<T> data) where T : class;

        public int Update(string tableName, string primaryKeyName, object primaryKeyValue, object data, IEnumerable<string>? columns = null);
       
        public int Update(string tableName, string primaryKeyName, object data, IEnumerable<string>? columns = null);
        
        public int Update<T>(T data, IEnumerable<string> columns) where T : class;

        public int Update<T>(T data, object primaryKeyValue, IEnumerable<string>? columns) where T : class;

        public int Update<T>(T data) where T : class;
      
        public int Update<T>(string sql, params object[] args) where T : class;


        public int Delete(string tableName, string primaryKeyName, object data);

        public int Delete<T>(string sql, params object[] args) where T : class;
  
        public int Delete<T>(object dataOrPrimaryKey) where T : class;

    }
}
