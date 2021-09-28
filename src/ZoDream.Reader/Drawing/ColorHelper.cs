using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Drawing
{
    public static class ColorHelper
    {
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return Color.FromArgb(a, r, g, b);
        }

        public static Color From(string color)
        {
            return ColorTranslator.FromHtml(color);
        }
    }
}
