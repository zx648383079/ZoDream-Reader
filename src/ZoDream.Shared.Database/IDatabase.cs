using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZoDream.Shared.Database.Adapters;

namespace ZoDream.Shared.Database
{
    public interface IDatabase: IDatabaseQuery, IDisposable
    {

        public IBuilderGrammar Grammar { get; }

        public object Insert<T>(string tableName, string primaryKeyName, 
            bool autoIncrement, T data);
   
        public object Insert<T>(string tableName, string primaryKeyName, T data);

        public object Insert<T>(T data);

        public int Update(string tableName, string primaryKeyName, object data, object primaryKeyValue);
               
        public int Update(string tableName, string primaryKeyName, object data);
        
        public int Update(string tableName, string primaryKeyName, object data, object? primaryKeyValue, IEnumerable<string>? columns);
       
        public int Update(string tableName, string primaryKeyName, object data, IEnumerable<string>? columns);
        
        public int Update(object data, IEnumerable<string> columns);
    
        public int Update(object data, object primaryKeyValue, IEnumerable<string>? columns);
      
        public int Update(object data);
      
        public int Update<T>(T poco, Expression<Func<T, object>> fields);

        public int Update(object data, object primaryKeyValue);
        
        public int Update<T>(string sql, params object[] args);


        public int Delete(string tableName, string primaryKeyName, object data);

        public int Delete(string tableName, string primaryKeyName, object? data, object? primaryKeyValue);

        public int Delete(object data);
 
        public int Delete<T>(string sql, params object[] args);
  
        public int Delete<T>(object dataOrPrimaryKey);

    }
}
