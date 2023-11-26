using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Database.Adapters;

namespace ZoDream.Shared.Database
{
    public  static class ParameterHelper
    {
        public static Regex RawParamsPrefix = new(@"(?<!@)@\w+", RegexOptions.Compiled);

        public static void SetParameterValue(IBuilderGrammar grammar, DbParameter p, object? value)
        {
            if (value is null)
            {
                p.Value = DBNull.Value;
                return;
            }
            value = grammar.MapParameterValue(value);

            var t = value.GetType();
            var underlyingT = Nullable.GetUnderlyingType(t);
            if (t.GetTypeInfo().IsEnum || (underlyingT != null && underlyingT.GetTypeInfo().IsEnum))        // PostgreSQL .NET driver wont cast enum to int
            {
                p.Value = (int)value;
            }
            else if (t == typeof(Guid))
            {
                p.Value = value;
                p.DbType = DbType.Guid;
                p.Size = 40;
            }
            else if (t == typeof(string))
            {
                var strValue = value as string;
                if (strValue == null)
                {
                    p.Size = 0;
                    p.Value = DBNull.Value;
                }
                else
                {
                    // out of memory exception occurs if trying to save more than 4000 characters to SQL Server CE NText column. Set before attempting to set Size, or Size will always max out at 4000
                    if (strValue.Length + 1 > 4000 && p.GetType().Name == "SqlCeParameter")
                    {
                        p.GetType().GetProperty("SqlDbType").SetValue(p, SqlDbType.NText, null);
                    }

                    p.Size = Math.Max(strValue.Length + 1, 4000); // Help query plan caching by using common size
                    p.Value = value;
                }
            }
            else if (value.GetType().Name == "SqlGeography") //SqlGeography is a CLR Type
            {
                p.GetType().GetProperty("UdtTypeName").SetValue(p, "geography", null); //geography is the equivalent SQL Server Type
                p.Value = value;
            }

            else if (value.GetType().Name == "SqlGeometry") //SqlGeometry is a CLR Type
            {
                p.GetType().GetProperty("UdtTypeName").SetValue(p, "geometry", null); //geography is the equivalent SQL Server Type
                p.Value = value;
            }
            else
            {
                p.Value = value;
            }
        }
    }
}
