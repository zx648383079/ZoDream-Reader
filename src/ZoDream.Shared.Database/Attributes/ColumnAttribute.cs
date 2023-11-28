using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;

        public int Length { get; set; }
        public bool ExactNameMatch { get; set; }

        public bool Nullable { get; set; }
    }
}
