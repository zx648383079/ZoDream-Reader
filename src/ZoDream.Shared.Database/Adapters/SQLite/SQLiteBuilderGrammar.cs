using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters.SQLite
{
    internal class SQLiteBuilderGrammar : SQLBuilderGrammar, IBuilderGrammar
    {
        protected override string CompileFieldType(TableField field)
        {
            return field.ValueType?.Name switch
            {
                "int" => "INTEGER",
                "object" => "BLOB",
                "float" or "double" => "REAL",
                "bool" => "NUMERIC",
                "null" => "NULL",
                _ => "TEXT",
            };
        }
    }
}
