using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public abstract class SQLBuilderGrammar
    {



        public void CompileCreateTable(StringBuilder builder, Table table)
        {
            builder.AppendLine($"CREATE TABLE IF NOT EXISTS {WrapName(table.Name)} (");
            foreach (var item in table.Items)
            {
                CompileCreateField(builder, item);
            }
            foreach (var item in table.Items)
            {
                if (item.IsPrimaryKey)
                {
                    builder.AppendLine($"PRIMARY KEY ({WrapName(item.Name)})");
                }
            }
            builder.AppendLine(");");
        }

        protected virtual void CompileCreateField(StringBuilder builder, TableField field)
        {
            var extra = field.Nullable ? "NULL" : "NOT NULL";
            if (field.AutoIncrement)
            {
                extra += " AUTO_INCREMENT";
            }
            if (field.Default is not null)
            {
                extra += " DEFAULT " + field.Default is string ? WrapText(field.Default) : field.Default;
            }
            builder.AppendLine($"{WrapName(field.Name)} {CompileFieldType(field)} {extra};");
        }

        protected abstract string CompileFieldType(TableField field);

        public string WrapTable(string table, string? schema = null)
        {
            var alias = string.Empty;
            var match = Regex.Match(table, @"(`?([\w_]+)`?\.)?(`?(!?[\w_]+)`?)(\s(as\s)?([\w_]+))?", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                alias = match.Groups[7].Value;
                if (string.IsNullOrEmpty(schema))
                {
                    schema = match.Groups[2].Value;
                }
                table = match.Groups[4].Value;
            }
            // var prefix = "";
            // var currentSchema = "";
            //if (schema == currentSchema)
            //{
            //    schema = string.Empty;
            //}
            if (table.StartsWith("!"))
            {
                table = table.Substring(1);
            }
            //else if (!string.IsNullOrWhiteSpace(prefix) && table.StartsWith(prefix))
            //{
            //    table = prefix + table;
            //}
            var res = string.IsNullOrWhiteSpace(schema) ? $"`{table}`" : $"`{schema}`.`{table}`";
            return string.IsNullOrWhiteSpace(alias) ? res : $"{res} AS {alias}";
        }

        public string WrapName(string name)
        {
            if (name.Contains('`'))
            {
                return name;
            }
            return $"`{name}`";
        }

        public string WrapText(object text)
        {
            return $"'{text}'";
        }
    }
}
