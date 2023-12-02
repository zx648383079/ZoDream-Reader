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
            return field.ValueType?.Name switch
            {
                "int" or "Int32" or "Int64" => "INT",
                "UInt32" => "INT UNSIGNED",
                "object" => "JSON",
                "float" or "Float" => "FLOAT",
                "double" or "Double" => "DOUBLE",
                // "float" or "double" or "Float" or "Double" => "DECIMAL(8,2)",
                "bool" or "Boolean" or "Byte" => "TINYINT(1) UNSIGNED",
                "DateTime" => "INT(10) UNSIGNED",
                "Enum" => "ENUM()",
                "null" => "NULL",
                "Char" => "CHAR(1)",
                "String" => "VARCHAR(255)",
                _ => "TEXT",
            };
        }
    }
}
