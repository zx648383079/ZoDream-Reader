using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute(string tableName) : Attribute
    {
        public string Value { get; private set; } = tableName;
    }
}
