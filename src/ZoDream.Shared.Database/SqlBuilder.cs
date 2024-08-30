using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Database.Mappers;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database
{
    public class SqlBuilder<T>(IDatabase database): ISqlBuilder<T>
    {

        private readonly Dictionary<string, SQLStringBuilder> Data = [];
        private bool _isEmpty = false;
        public IDatabase Database { get; private set; } = database;

        public void Append(string name, string sql, object[] parameters, string joiner)
        {
            if (!Data.TryGetValue(name, out var builder))
            {
                builder = new SQLStringBuilder(name.ToUpper());
                Data.Add(name, builder);
            }
            builder.Append(joiner.ToUpper(), sql, parameters);
        }

        public void Clear(string name)
        {
            if (Data.TryGetValue(name, out var builder))
            {
                builder.Clear();
            }
        }

        public void Clear()
        {
            foreach (var item in Data)
            {
                item.Value.Clear();
            }
        }
        /// <summary>
        /// 生成值占位符
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ParameterPlaceholder(params object[] val)
        {
            return ParameterPlaceholder(val.Length);
        }
        /// <summary>
        /// 生成值占位符
        /// </summary>
        /// <param name="count"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ParameterPlaceholder(int count, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(separator);
                }
                sb.Append("?");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 替换值占位符
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string ParameterFormat(string sql)
        {
            var i = -1;
            return Regex.Replace(sql, @"\?", _ => {
                i++;
                return $"{Database.Grammar.ParamPrefix}{i} ";
            });
        }

        public void FromIfEmpty()
        {
            if (Data.ContainsKey("from"))
            {
                return;
            }
            From(ReflectionHelper.GetTableName(typeof(T)));
        }

        public bool Any()
        {
            return Count() > 0;
        }

        public double Average(string key)
        {
            Clear("select");
            return SelectCall("AVG", key, "avg").Value<int>("avg");
        }

        public int Count()
        {
            return Count("*");
        }

        public int Count(string key)
        {
            if (_isEmpty)
            {
                return 0;
            }
            Clear("select");
            return SelectCall("COUNT", key, "count").Value<int>("count");
        }

        public int Delete()
        {
            FromIfEmpty();
            var builder = Database.Grammar.CompileDelete(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public T? First()
        {
            if (!Data.ContainsKey("select"))
            {
                Select("*");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileSelect(Data);
            using var cmd = Database.CreateCommand(Database.Connection, CommandType.Text, 
                ParameterFormat(builder.ToString()), builder.Parameters);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return default;
            }
            return (T)new TypeMapper().Map(reader, typeof(T))!;
        }

        public T FirstOrDefault()
        {
            var data = First();
            if (data is null)
            {
                return Activator.CreateInstance<T>();
            }
            return data;
        }


        public ISqlBuilder<T> From<K>() 
            where K : class
        {
            return From(ReflectionHelper.GetTableName(typeof(K)));
        }

        public ISqlBuilder<T> From(string tableName)
        {
            Append("from", Database.Grammar.WrapTable(tableName), [], ",");
            return this;
        }


        public ISqlBuilder<T> GroupBy(string key)
        {
            Append("group by", Database.Grammar.WrapName(key), [], ",");
            return this;
        }

        public object? Insert(IDictionary<string, object> data)
        {
            foreach(var item in  data)
            {
                Append("field", Database.Grammar.WrapName(item.Key), [], ",");
                Append("values", "?", [item.Value], ",");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileInsert(Data);
            return Database.ExecuteScalar<string>(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public object? Insert(T data)
        {
            var (field, atts) = ReflectionHelper.GetProperties(typeof(T));
            foreach(var key in field)
            {
                Append("field", Database.Grammar.WrapName(key), [], ",");
            }
            foreach (var item in atts)
            {
                Append("values", "?", [item.GetValue(data)], ",");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileInsert(Data);
            return Database.ExecuteScalar<string>(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int Insert(IEnumerable<T> items)
        {
            var (field, atts) = ReflectionHelper.GetProperties(typeof(T));
            foreach (var key in field)
            {
                Append("field", Database.Grammar.WrapName(key), [], ",");
            }
            var i = 0;
            foreach (var item in items)
            {
                var name = $"values {i++}";
                foreach(var attr in atts)
                {
                    Append(name, "?", [attr.GetValue(item)], ",");
                }
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileInsert(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public ISqlBuilder<T> IsEmpty()
        {
            _isEmpty = true;
            return this;
        }

        public ISqlBuilder<T> Limit(int size)
        {
            return Take(size);
        }

        public ISqlBuilder<T> Limit(int begin, int size)
        {
            return Skip(begin).Take(size);
        }

        public long LongCount()
        {
            if (_isEmpty)
            {
                return 0;
            }
            Clear("select");
            return SelectCall("COUNT", "*", "count").Scalar<long>()!;
        }

        public K Max<K>(string key)
        {
            Clear("select");
            return SelectCall("MAX", key, "max").Scalar<K>()!;
        }

        public K Min<K>(string key)
        {
            Clear("select");
            return SelectCall("MIN", key, "min").Scalar<K>()!;
        }


        public ISqlBuilder<T> OrderBy(string key, bool desc = false)
        {
            key = Database.Grammar.WrapName(key);
            var sort = desc ? "DESC" : "ASC";
            Append("order by", $"{key} {sort}", [], ",");
            return this;
        }

        public ISqlBuilder<T> OrderByAsc(string key)
        {
            return OrderBy(key, false);
        }

        public ISqlBuilder<T> OrderByDesc(string key)
        {
            return OrderBy(key, true);
        }

        public ISqlBuilder<T> OrWhere(string key, object val)
        {
            return OrWhere(key, "=", val);
        }

        public ISqlBuilder<T> OrWhere(string key, string @operator, object val)
        {
            @operator = @operator.ToUpper();
            switch (@operator)
            {
                case "IN":
                    return OrWhereIn(key, (object[])val);
                case "NOT IN":
                    return OrWhereIn(key, (object[])val);
                default:
                    break;
            }
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} {@operator} ?", [val], "or");
            return this;
        }

        public ISqlBuilder<T> OrWhereBetween(string key, object begin, object end)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} BETWEEN ? AND ?", [begin, end], "or");
            return this;
        }

        public ISqlBuilder<T> OrWhereIn(string key, params object[] val)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IN ({ParameterPlaceholder(val)})", val, "or");
            return this;
        }

        public ISqlBuilder<T> OrWhereLike(string key, string val)
        {
            return OrWhere(key, "like", val);
        }

        public ISqlBuilder<T> OrWhereNotIn(string key, params object[] val)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} NOT IN ({ParameterPlaceholder(val)})", val, "or");
            return this;
        }

        public ISqlBuilder<T> OrWhereNotNull(string key)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IS NOT NULL", [], "or");
            return this;
        }

        public ISqlBuilder<T> OrWhereNull(string key)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IS NULL", [], "or");
            return this;
        }

        public Page<T> Page(long page, long perPage)
        {
            FromIfEmpty();
            Clear("offset");
            Clear("limit");
            if (Data.TryGetValue("select", out var select))
            {
                Data["select"] = new SQLStringBuilder("select");
            }
            var total = LongCount();
            var offset = (page - 1) * perPage;
            if (total < offset)
            {
                return new Page<T>() 
                { 
                    CurrentPage = page,
                    TotalItems = total,
                    PerPage = perPage,
                    Items = []
                };
            }
            if (select is null)
            {
                Clear("select");
                Select("*");
            } else
            {
                Data["select"] = select;
            }
            Limit((int)offset, (int)perPage);
            var builder = Database.Grammar.CompileSelect(Data);
            var items = Database.Fetch<T>(builder.ToString(), builder.Parameters);
            return new Page<T>()
            {
                CurrentPage = page,
                TotalItems = total,
                PerPage = perPage,
                Items = items
            };
        }

        public K? Scalar<K>()
        {
            if (!Data.ContainsKey("select"))
            {
                Select("*");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileSelect(Data);
            return Database.ExecuteScalar<K>(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public ISqlBuilder<T> Select(params string[] items)
        {
            foreach (var item in items)
            {
                Append("select", item == "*" ? item : Database.Grammar.WrapName(item), [], ",");
            }
            return this;
        }

        public ISqlBuilder<T> SelectCall(string func, string key, string alias)
        {
            if (key != "*")
            {
                key = Database.Grammar.WrapName(key);
            }
            Append("select", $"{func}({key}) AS {alias}", [], ",");
            return this;
        }

        public ISqlBuilder<T> Skip(int amount)
        {
            Clear("offset");
            Append("offset", amount.ToString(), [], string.Empty);
            return this;
        }

        public K Sum<K>(string key)
        {
            Clear("select");
            return SelectCall("SUM", key, "sum").Scalar<K>()!;
        }

        public ISqlBuilder<T> Take(int count)
        {
            Clear("limit");
            Append("limit", count.ToString(), [], string.Empty);
            return this;
        }

        public T[] ToArray()
        {
            return [.. ToList()];
        }

        public IList<T> ToList()
        {
            if (_isEmpty)
            {
                return [];
            }
            if (!Data.ContainsKey("select"))
            {
                Select("*");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileSelect(Data);
            return Database.Fetch<T>(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int Update(T data)
        {
            var type = typeof(T);
            var key = ReflectionHelper.GetPrimaryKey(type);
            foreach (var attr in type.GetProperties())
            {
                var name = ReflectionHelper.GetPropertyName(attr);
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                var val = attr.GetValue(data);
                if (name == key)
                {
                    continue;
                }
                Append("set", $"{Database.Grammar.WrapName(name)}=?", [val], ",");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int Update(string key, object val)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key}=?", [val], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int Update(IDictionary<string, object> data)
        {
            foreach (var item in data)
            {
                Append("set", $"{Database.Grammar.WrapName(item.Key)}=?", [item.Value], ",");
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateBool(string key)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = CASE WHEN {key} = 1 THEN 0 ELSE 1 END", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateDecrement(string key, int offset = 1)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} - {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateDecrement(string key, float offset)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} - {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateDecrement(string key, double offset)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} - {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateIncrement(string key, int offset = 1)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} + {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateIncrement(string key, float offset)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} + {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public int UpdateIncrement(string key, double offset)
        {
            key = Database.Grammar.WrapName(key);
            Append("set", $"{key} = {key} + {offset}", [], ",");
            FromIfEmpty();
            var builder = Database.Grammar.CompileUpdate(Data);
            return Database.Execute(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public K? Value<K>(string key)
        {
            if (!Data.ContainsKey("select"))
            {
                Select(key);
            }
            FromIfEmpty();
            var builder = Database.Grammar.CompileSelect(Data);
            return Database.ExecuteScalar<K>(ParameterFormat(builder.ToString()), builder.Parameters);
        }

        public ISqlBuilder<T> When(bool condition, Action<ISqlBuilder<T>> trueFunc)
        {
            if (condition)
            {
                trueFunc.Invoke(this);
            }
            return this;
        }

        public ISqlBuilder<T> When(bool condition, Action<ISqlBuilder<T>> trueFunc, Action<ISqlBuilder<T>> falseFunc)
        {
            if (condition)
            {
                trueFunc.Invoke(this);
            } else
            {
                falseFunc.Invoke(this);
            }
            return this;
        }


        public ISqlBuilder<T> Where(string key, object val)
        {
            return Where(key, "=", val);
        }

        public ISqlBuilder<T> Where(string key, string @operator, object val)
        {
            @operator = @operator.ToUpper();
            switch (@operator)
            {
                case "IN":
                    return WhereIn(key, (object[])val);
                case "NOT IN":
                    return WhereIn(key, (object[])val);
                default:
                    break;
            }
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} {@operator} ?", [val], "and");
            return this;
        }

        public ISqlBuilder<T> WhereBetween(string key, object begin, object end)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} BETWEEN ? AND ?", [begin, end], "and");
            return this;
        }

        public ISqlBuilder<T> WhereIn(string key, params object[] val)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IN ({ParameterPlaceholder(val)})", val, "and");
            return this;
        }

        public ISqlBuilder<T> WhereLike(string key, string val)
        {
            return Where(key, "like", val);
        }

        public ISqlBuilder<T> WhereNotIn(string key, params object[] val)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} NOT IN ({ParameterPlaceholder(val)})", val, "and");
            return this;
        }

        public ISqlBuilder<T> WhereNotNull(string key)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IS NOT NULL", [], "and");
            return this;
        }

        public ISqlBuilder<T> WhereNull(string key)
        {
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} IS NULL", [], "and");
            return this;
        }

        public ISqlBuilder<T> Having(string key, object val)
        {
            return Having(key, "=", val);
        }
        public ISqlBuilder<T> Having(string key, string @operator, object val)
        {
            @operator = @operator.ToUpper();
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} {@operator} ?", [val], "and");
            return this;
        }

        public ISqlBuilder<T> OrHaving(string key, object val)
        {
            return OrHaving(key, "=", val);
        }
        public ISqlBuilder<T> OrHaving(string key, string @operator, object val)
        {
            @operator = @operator.ToUpper();
            key = Database.Grammar.WrapName(key);
            Append("where", $"{key} {@operator} ?", [val], "or");
            return this;
        }
    }
}
