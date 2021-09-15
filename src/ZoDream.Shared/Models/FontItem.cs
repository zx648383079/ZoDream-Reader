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

        public FontItem(string name)
        {
            Name = name;
        }
    }
}
