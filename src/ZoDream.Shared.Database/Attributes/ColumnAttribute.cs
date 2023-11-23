using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute: Attribute
    {
        public ColumnAttribute(string name) 
        { 
            Name = name; 
        }
        public string Name { get; set; }
        public bool ExactNameMatch { get; set; }
    }
}
