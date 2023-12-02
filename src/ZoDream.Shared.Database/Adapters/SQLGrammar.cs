using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public abstract partial class SQLGrammar
    {

        public string ParamPrefix { get; private set; } = "@";

        public string CompileSelect(string tableName)
        {
            return $"SELECT * FROM {WrapName(tableName)}";
        }

        public string CompileSelectJoin(string tableName, string sql, long page, long perPage)
        {
            var offset = Math.Max((page - 1) * perPage, 0);
            if (sql.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
            {
                return $"{sql} LIMIT {perPage} OFFSET {offset}";
            }
            return $"SELECT * FROM {WrapName(tableName)} {sql} LIMIT {perPage} OFFSET {offset}";
        }

        public string CompileDelete(string tableName, string primaryKeyName)
        {
            return $"DELETE FROM {WrapName(tableName)} WHERE {WrapName(primaryKeyName)}={ParamPrefix}0";
        }

        public string CompileDeleteJoin(string tableName, string sql)
        {
            if (sql.StartsWith("DELETE", StringComparison.CurrentCultureIgnoreCase))
            {
                return sql;
            }
            return $"DELETE FROM {WrapName(tableName)} {sql}";
        }

        public string CompileUpdate(string tableName, string primaryKeyName, IEnumerable<string> columns)
        {
            var sql = string.Empty;
            var i = 0;
            foreach (var item in columns)
            {
                if (i > 0)
                {
                    sql += ", ";
                }
                i++;
                sql += $"{WrapName(item)}={ParamPrefix}{i}";
            }
            return $"UPDATE {WrapName(tableName)} SET {sql} WHERE {WrapName(primaryKeyName)}={ParamPrefix}0";
        }

        public string CompileUpdateJoin(string tableName, string sql)
        {
            if (sql.StartsWith("UPDATE", StringComparison.CurrentCultureIgnoreCase))
            {
                return sql;
            }
            return $"UPDATE {WrapName(tableName)} {sql}";
        }

        public string CompileInsert(string tableName, string primaryKeyName, List<string> columns)
        {
            var sql = CompileInsert(tableName, columns);
            if (columns.Contains(primaryKeyName))
            {
                return sql;
            }
            return $"{sql}{CompileLastInsertId()}";
        }

        public string CompileInsert(string tableName, List<string> columns, int begin = 0)
        {
            var field = string.Join(",", columns.Select(WrapName));
            var value = string.Join(",", columns.Select((_, i) => ParamPrefix + (begin + i)));
            return $"INSERT INTO {WrapName(tableName)}({field}) VALUES({value});";
        }

        public SQLStringBuilder CompileUpdate(Dictionary<string, SQLStringBuilder> data)
        {
            var sb = new SQLStringBuilder("UPDATE");
            sb.Append(data["from"], false);
            var maps = new[] { "set", "where", "limit", "offset" };
            foreach (var name in maps)
            {
                if (data.TryGetValue(name, out var item))
                {
                    sb.Append(item);
                }
            }
            return sb;
        }

        public SQLStringBuilder CompileInsert(Dictionary<string, SQLStringBuilder> data)
        {
            var sb = new SQLStringBuilder("INSERT INTO");
            sb.Append(data["from"], false)
                .Append("(")
                .Append(data["field"], false)
                .Append(") VALUES");
            var isFirst = true;
            foreach (var item in data)
            {
                if (item.Key.StartsWith("values "))
                {
                    if (!isFirst)
                    {
                        sb.Append(",");
                    }
                    sb.Append("(").Append(item.Value, false).Append(")");
                    isFirst = false;
                }
            }
            return sb.Append(";");
        }

        public SQLStringBuilder CompileDelete(Dictionary<string, SQLStringBuilder> data)
        {
            var sb = new SQLStringBuilder("DELETE");
            var maps = new[] { "from", "where", "limit", "offset" };
            foreach (var name in maps)
            {
                if (data.TryGetValue(name, out var item))
                {
                    sb.Append(item);
                }
            }
            return sb;
        }
        public SQLStringBuilder CompileSelect(Dictionary<string, SQLStringBuilder> data)
        {
            var maps = new[] { "select", "from", "where", "group by", "having", "order by", "limit", "offset"};
            var sb = new SQLStringBuilder(string.Empty);
            foreach (var name in maps)
            {
                if (data.TryGetValue(name, out var item))
                {
                    sb.Append(item);
                }
            }
            return sb;
        }

        public void CompileCreateTable(StringBuilder builder, Table table)
        {
            builder.AppendLine($"CREATE TABLE IF NOT EXISTS {WrapName(table.Name)} (");
            var isFirst = true;
            foreach (var item in table.Items)
            {
                CompileCreateField(builder, item, isFirst);
                isFirst = false;
            }
            foreach (var item in table.Items)
            {
                CompileFieldExtra(builder, item);
            }
            builder.AppendLine(");");
        }

        public string CompileDropTable(string tableName)
        {
            return $"DROP TABLE IF EXISTS {WrapName(tableName)};";
        }
        public string CompileDropTable(Table table)
        {
            return CompileDropTable(table.Name);
        }

        protected virtual string CompileLastInsertId()
        {
            return "SELECT LAST_INSERT_ID();";
        }
        protected virtual void CompileFieldExtra(StringBuilder builder,
            TableField field)
        {
            if (field.IsPrimaryKey)
            {
                builder.AppendLine($",PRIMARY KEY ({WrapName(field.Name)})");
            }
        }
        protected virtual void CompileCreateField(StringBuilder builder, 
            TableField field, bool isFristLine)
        {
            var extra = field.Nullable ? "NULL" : "NOT NULL";
            if (field.AutoIncrement)
            {
                extra += " AUTO_INCREMENT";
            }
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
