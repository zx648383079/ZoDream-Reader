using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZoDream.Reader.Helper
{
    public class UrlHelper
    {
        public static string GetWeb(string url)
        {
            return Regex.Match(url, @"[hH][Tt]{2}[pP][sS]?://[\w\.]+").Value;
        }

        public static string GetAbsolute(string url, string relative)
        {
            return new Uri(new Uri(url), relative).ToString();
        }
    }
}
