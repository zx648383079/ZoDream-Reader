using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database.Models
{
    public class SQLStringBuilder(string prefix)
    {
        public string Prefix { get; private set; } = prefix;
        public List<object> Parameters = [];
        public StringBuilder Sql = new();

        public bool IsEmpty => Sql.Length == 0;

        public SQLStringBuilder Append(string joiner, string sql, params object[] args)
        {
            if (!IsEmpty)
            {
                Sql.Append(joiner).Append(' ');
            }
            Sql.Append(sql);
            Parameters.AddRange(args);
            return this;
        }

        public SQLStringBuilder Append(SQLStringBuilder builder)
        {
            return Append(builder, true);
        }

        public SQLStringBuilder Append(SQLStringBuilder builder, bool addPrefix)
        {
            if (builder.IsEmpty)
            {
                return this;
            }
            if (!IsEmpty)
            {
                Sql.Append(' ');
            }
            Sql.Append(addPrefix ? builder.ToString() : builder.Sql.ToString());
            Parameters.AddRange(builder.Parameters);
            return this;
        }

        public SQLStringBuilder Append(StringBuilder builder)
        {
            if (builder.Length == 0)
            {
                return this;
            }
            if (!IsEmpty)
            {
                Sql.Append(' ');
            }
            Sql.Append(builder.ToString());
            return this;
        }
        public SQLStringBuilder Append(string builder)
        {
            if (string.IsNullOrWhiteSpace(builder))
            {
                return this;
            }
            if (!IsEmpty)
            {
                Sql.Append(' ');
            }
            Sql.Append(builder);
            return this;
        }

        public void Clear()
        {
            Sql.Clear();
            Parameters.Clear();
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(Prefix))
            {
                return Sql.ToString();
            }
            return $"{Prefix} {Sql}";
        }
    }
}
