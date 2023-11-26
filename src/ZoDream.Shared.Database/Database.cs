using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
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
        private string _paramPrefix = "@";

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

        public virtual void AddParameter(DbCommand cmd, object? value)
        {
            if (value is DbParameter idbParam)
            {
                idbParam.ParameterName = string.Format("{0}{1}", _paramPrefix, cmd.Parameters.Count);
                cmd.Parameters.Add(idbParam);
                return;
            }

            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("{0}{1}", _paramPrefix, cmd.Parameters.Count);

            ParameterHelper.SetParameterValue(Grammar, p, value);

            cmd.Parameters.Add(p);
        }

        // Create a command
        private DbCommand CreateCommand(DbConnection connection, string sql, params object[] args)
        {
            return CreateCommand(connection, CommandType.Text, sql, args);
        }

        public virtual DbCommand CreateCommand(DbConnection connection, CommandType commandType, string sql, params object[] args)
        {
            if (commandType == CommandType.StoredProcedure)
            {
                return CreateStoredProcedureCommand(connection, sql, args);
            }
            if (_paramPrefix != "@")
            {
                sql = ParameterHelper.RawParamsPrefix.Replace(sql, m => _paramPrefix + m.Value.Substring(1));
            }
            sql = sql.Replace("@@", "@");
            DbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;
            // cmd.Transaction = _transaction;

            foreach (var item in args)
            {
                AddParameter(cmd, item);
            }
            return cmd;
        }

        public DbCommand CreateStoredProcedureCommand(DbConnection connection, string name, params object[] args)
        {
            DbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            // cmd.Transaction = _transaction;

            if (args.Length == 1)
            {
                if (args[0] is DbParameter arg)
                {
                    cmd.Parameters.Add(arg);
                }
                else
                {
                    var props = args[0].GetType().GetProperties().Select(x => new { x.Name, Value = x.GetValue(args[0], null) }).ToList();
                    foreach (var item in props)
                    {
                        DbParameter param = cmd.CreateParameter();
                        param.ParameterName = item.Name;

                        ParameterHelper.SetParameterValue(Grammar, param, item.Value);

                        cmd.Parameters.Add(param);
                    }
                }
            }
            else
            {
                cmd.Parameters.AddRange(args.OfType<DbParameter>().ToArray());
            }
            return cmd;
        }

        public IQuery<T> Query<T>()
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder<T> Build<T>()
        {
            return new SqlBuilder<T>(this);
        }

        public void Dispose()
        {
            _sharedConnection.Close();
            _sharedConnection.Dispose();
        }
    }
}
