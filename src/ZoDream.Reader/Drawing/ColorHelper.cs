using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Drawing
{
    public static class ColorHelper
    {
        public static RawColor4 FromArgb(byte a, byte r, byte g, byte b)
        {
            return new RawColor4(r / 255, g / 255, b / 255, a / 255);
        }
    }
}
