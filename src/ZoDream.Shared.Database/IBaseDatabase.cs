using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;

namespace ZoDream.Shared.Database
{
    public interface IBaseDatabase: IDisposable
    {
        public DbCommand CreateCommand(DbConnection connection, CommandType commandType, string sql, params object[] args);

        public void AddParameter(DbCommand cmd, object value);

    }
}
