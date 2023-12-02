using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters.SQLite
{
    internal class SQLiteBuilderGrammar : SQLGrammar, IBuilderGrammar, ILinqGrammar
    {

        protected override void CompileFieldExtra(StringBuilder builder, TableField field)
        {
            if (field.IsPrimaryKey)
            {
                var extra = field.AutoIncrement ? " AUTOINCREMENT" : "";
                builder.AppendLine($",PRIMARY KEY ({WrapName(field.Name)}{extra})");
            }
        }
        protected override void CompileCreateField(StringBuilder builder,
            TableField field, bool isFristLine)
        {
            var extra = field.Nullable ? "NULL" : "NOT NULL";
            if (field.IsUnique)
            {
                extra += "UNIQUE";
            }
            if (field.Default is not null)
            {
                extra += " DEFAULT " + field.Default is string ? WrapText(field.Default) : field.Default;
            }

            if (!isFristLine)
            {
                builder.Append(',');
            }
            builder.AppendLine($"{WrapName(field.Name)} {CompileFieldType(field)} {extra}");
        }
        public object MapParameterValue(object value)
        {
            return value;
        }

        protected override string CompileFieldType(TableField field)
        {
            return field.ValueType?.Name switch
            {
                "int" or "Int32" or "Int64" => "INTEGER",
                "object" => "BLOB",
                "float" or "double" or "Float" or "Double" => "REAL",
                "bool" or "Boolean" => "NUMERIC",
                "null" => "NULL",
                _ => "TEXT",
            };
        }
    }
}
