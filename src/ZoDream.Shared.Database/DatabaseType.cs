using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database.Adapters;
using ZoDream.Shared.Database.Adapters.MySQL;
using ZoDream.Shared.Database.Adapters.SQLite;

namespace ZoDream.Shared.Database
{
    public static class DatabaseType
    {

        public static IBuilderGrammar Resolve(string typeName, string? providerName)
        {
            if (typeName.StartsWith("MySql"))
            {
                return new MySQLBuilderGrammar();
            }
            if (typeName.StartsWith("SQLite", StringComparison.OrdinalIgnoreCase))
            {
                return new SQLiteBuilderGrammar();
            }
            if (!string.IsNullOrEmpty(providerName))
            {
                // Try again with provider name
                if (providerName!.IndexOf("MySql", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return new MySQLBuilderGrammar();
                }
                if (providerName.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return new SQLiteBuilderGrammar();
                }
            }
            throw new ArgumentException();
        }
    }
}
