using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class CharItem
    {
        public char Code { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public override string ToString()
        {
            return $"{Code} X:{X},Y:{Y}";
        }
    }
}
