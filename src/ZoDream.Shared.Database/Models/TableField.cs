using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database.Models
{
    public class TableField
    {

        public string Name { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public Type? ValueType { get; set; }

        public int Length { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool AutoIncrement { get; set; }

        public bool IsUnique { get; set; }

        public object? Default { get; set; }

        public bool Nullable { get; set; }
    }
}
