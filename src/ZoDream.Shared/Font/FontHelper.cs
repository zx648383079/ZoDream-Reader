using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Font
{
    public class FontHelper
    {

        public static IList<string> GetFontFamily(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
            {
                return GetFontFamily(fs);
            }
        }

        public static IList<string> GetFontFamily(Stream fs)
        {
            var items = new List<string>();


            return items;
        }
    }
}
