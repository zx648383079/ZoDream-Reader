using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters.MySQL
{
    internal class MySQLBuilderGrammar : SQLBuilderGrammar, IBuilderGrammar
    {
        protected override string CompileFieldType(TableField field)
        {
            throw new NotImplementedException();
        }
    }
}
