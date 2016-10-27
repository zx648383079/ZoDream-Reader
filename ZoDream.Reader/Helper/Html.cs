using System;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Helper.Http;

namespace ZoDream.Reader.Helper
{
    public class HtmlExpand : Html
    {
        public string GetCover(string begin, string end)
        {
            return GetMatch(begin + @"[\s\S]+?[sS][Rr][Cc][\s]?=[\s""]?(?<src>[^""\<\>\s]+)" + end, "src");
        }

        public string GetAuthor(string begin, string end)
        {
            return ReplaceHtml(GetMatch(begin + @"([\s\S]+?)" + end, 1));
        }

        public string GetDescription(string begin, string end)
        {
            return ReplaceHtml(GetMatch(begin + @"([\s\S]+?)" + end, 1));
        }
    }
}
