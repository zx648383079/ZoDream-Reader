using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;

namespace ZoDream.Shared.Database
{
    public partial class Database : IDatabase
    {
        public Database(DbConnection connection)
        {
            _sharedConnection = connection;
        }

        public Database(string connectionString, DbProviderFactory provider)
        {
            _sharedConnection = provider.CreateConnection();
            _sharedConnection.ConnectionString = connectionString;
        }

        private DbConnection _sharedConnection;

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
