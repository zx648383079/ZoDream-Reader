using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute: Attribute
    {
        public TableNameAttribute(string tableName)
        {
            Value = tableName;
        }
        public string Value { get; private set; }
    }
}
