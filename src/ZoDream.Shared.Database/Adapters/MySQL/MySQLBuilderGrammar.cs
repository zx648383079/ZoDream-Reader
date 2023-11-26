using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters.MySQL
{
    internal class MySQLBuilderGrammar : SQLGrammar, IBuilderGrammar, ILinqGrammar
    {
        public object MapParameterValue(object value)
        {
            return value;
        }
        protected override string CompileFieldType(TableField field)
        {
            throw new NotImplementedException();
        }
    }
}
