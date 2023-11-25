using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using ZoDream.Shared.Database.Adapters;

namespace ZoDream.Shared.Database
{
    public partial class Database : IDatabase
    {
        public Database(DbConnection connection)
            : this(connection, DatabaseType.Resolve(connection.GetType().Name, null))
        {
        }

        public Database(string connectionString, DbProviderFactory provider)
        {
            _sharedConnection = provider.CreateConnection();
            _sharedConnection.ConnectionString = connectionString;
            Grammar = DatabaseType.Resolve(_sharedConnection.GetType().Name, null);
        }

        public Database(DbConnection connection, IBuilderGrammar grammar)
        {
            _sharedConnection = connection;
            Grammar = grammar;
        }

        private DbConnection _sharedConnection;

        public IBuilderGrammar Grammar { get; private set; }

        public void Open()
        {
            if (_sharedConnection.State != ConnectionState.Broken && 
                _sharedConnection.State != ConnectionState.Closed)
            {
                return;
            }
            if (_sharedConnection.State == ConnectionState.Broken)
            {
                _sharedConnection.Close();
            }
            if (_sharedConnection.State == ConnectionState.Closed)
            {
                _sharedConnection.Open();
            }
        }

        public IQuery<T> Query<T>()
        {
            throw new NotImplementedException();
        }
    }
}
