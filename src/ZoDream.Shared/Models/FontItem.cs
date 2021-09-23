using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Models
{
    public class FontItem
    {
        public string Name { get; set; }

        public string? FileName { get; set; }

        public string FontFamily
        {
            get {
                if (string.IsNullOrEmpty(FileName))
                {
                    return Name;
                }
                return $"{FileName}#{Name}";
            }
        }

        public override string ToString()
        {
            return FontFamily;
        }

        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        public FontItem(string name)
        {
            var args = name.Split('#');
            if (args.Length == 1)
            {
                Name = args[0];
                return;
            }
            Name = args[1];
            FileName = args[0];
        }
    }
}
