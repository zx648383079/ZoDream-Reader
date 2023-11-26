using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public interface IBuilderGrammar
    {

        public void CompileCreateTable(StringBuilder builder, Table table);

        public string CompileDropTable(string tableName);
        public string CompileDropTable(Table table);

        public object MapParameterValue(object value);

    }
}
