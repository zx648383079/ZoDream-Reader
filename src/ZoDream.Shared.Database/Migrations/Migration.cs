using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Migrations
{
    public abstract class Migration(IDatabase database) : IMigration
    {
        public IDatabase Database { get; private set; } = database;

        public abstract void Down();
        public abstract void Seed();
        public abstract void Up();

        public string CreateTable<T>()
            where T : class
        {
            var sb = new StringBuilder();
            CreateTable<T>(sb);
            return sb.ToString();
        }

        public void CreateTable<T>(StringBuilder builder)
            where T : class
        {
            var table = Format<T>();
            Database.Grammar.CompileCreateTable(builder, table);
        }


        public string DropTable<T>() where T : class
        {
            var sb = new StringBuilder();
            DropTable<T>(sb);
            return sb.ToString();
        }

        public void DropTable<T>(StringBuilder builder) where T : class
        {
            var name = ReflectionHelper.GetTableName(typeof(T));
            builder.AppendLine(Database.Grammar.CompileDropTable(name));
        }

        public Table Format<T>()
            where T : class
        {
            var type = typeof(T);
            var table = new Table() 
            {
                Name = ReflectionHelper.GetTableName(type),
            };
            var keys = new Dictionary<string, bool>();
            foreach (var item in type.GetCustomAttributes<PrimaryKeyAttribute>())
            {
                foreach (var key in item.Value.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        continue;
                    }
                    if (keys.ContainsKey(key))
                    {
                        keys[key] = item.AutoIncrement;
                        continue;
                    }
                    keys.Add(key, item.AutoIncrement);
                }
            }
            foreach (var item in type.GetProperties())
            {
                var name = ReflectionHelper.GetPropertyName(item);
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                var isPrimaryKey = false;
                if (keys.TryGetValue(name, out bool autoIncrement) || keys.TryGetValue(item.Name, out autoIncrement))
                {
                    isPrimaryKey = true;
                }
                var info = item.GetCustomAttribute<ColumnAttribute>();
                table.Items.Add(new TableField()
                {
                    Name = name,
                    ValueType = item.PropertyType,
                    IsPrimaryKey = isPrimaryKey,
                    AutoIncrement = autoIncrement,
                    Length = info is null ? 0 : info.Length,
                });
            }
            return table;
        }

    }
}
