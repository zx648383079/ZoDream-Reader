using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public interface IBuilderGrammar
    {

        public string ParamPrefix { get; }

        public string CompileSelect(string tableName);
        public string CompileSelectJoin(string tableName, string sql, long page, long perPage);
        
        public string CompileDelete(string tableName, string primaryKeyName);
        public string CompileDeleteJoin(string tableName, string sql);
        public string CompileUpdate(string tableName, string primaryKeyName, IEnumerable<string> columns);
        public string CompileUpdateJoin(string tableName, string sql);

        public string CompileInsert(string tableName, string primaryKeyName, List<string> columns);
        public string CompileInsert(string tableName, List<string> columns, int begin = 0);

        public void CompileCreateTable(StringBuilder builder, Table table);

        public string CompileDropTable(string tableName);
        public string CompileDropTable(Table table);
        public object MapParameterValue(object value);
        
    }
}
