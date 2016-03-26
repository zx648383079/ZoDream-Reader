using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Model
{
    public class OptionItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public OptionItem()
        {
            
        }

        public OptionItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
