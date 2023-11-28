using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class DataItem(string name, object val)
    {
        public string Name { get; set; } = name;

        public object Value { get; set; } = val;
    }
}
