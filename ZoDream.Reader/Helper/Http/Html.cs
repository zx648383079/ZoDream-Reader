using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.Helper.Http
{
    public class Html
    {
        private string _html;

        public Html()
        {
            
        }

        public Html(string html)
        {
            _html = html;
        }

        public Html SetUrl(string url)
        {
            var request = new Request();
            _html = request.Get(url);
            return this;
        }

        /// <summary>
        /// 缩小范围
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public Html Match(string pattern)
        {
            _html = Regex.Match(_html, pattern).Value;
            return this;
        }

        public Html Match(string begin, string end)
        {
            return Match(begin + @"[\s\S]+?" + end);
        }

        public MatchCollection Matches(string pattern)
        {
            return Regex.Matches(_html, pattern, RegexOptions.IgnoreCase);
        }

        public string GetMatch(string pattern, string tag)
        {
            return Regex.Match(_html, pattern).Groups[tag].Value;
        }

        public string GetMatch(string pattern, int tag)
        {
            return Regex.Match(_html, pattern).Groups[tag].Value;
        }

        public string GetCover(string begin, string end)
        {
            return GetMatch(begin + @"[\s\S]+?[sS][Rr][Cc][\s]?=[\s""]?(?<src>[^""\<\>\s]+)" + end, "src");
        }

        public string GetAuthor(string begin, string end)
        {
            return ReplaceHtml(GetMatch(begin + @"([\s\S]+?)"+ end, 1));
        }

        public string GetDescription(string begin, string end)
        {
            return ReplaceHtml(GetMatch(begin + @"([\s\S]+?)" + end, 1));
        }

        public string ReplaceHtml(string html)
        {
            html = Regex.Replace(html, @"\s+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--[\s\S]*-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<(script|style)[^>]*?>.*?</\1>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<(br|p)[^>]*>", "\n", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<[^>]*>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#\d+;", "", RegexOptions.IgnoreCase);
            return html;
        }

        /// <summary>
        /// 自定义替换规则 = ||
        /// </summary>
        /// <param name="replace"></param>
        /// <returns></returns>
        public void Replace(string replace)
        {
            var ms = Regex.Matches(replace, @"([^(=|\|\|)]+)(=([^(=|\|\|)]*))?");
            foreach (Match match in ms)
            {
                _html = Regex.Replace(_html, match.Groups[1].Value, match.Groups[3].Value, RegexOptions.IgnoreCase);
            }
        }

        public List<string> GetLinks()
        {
            return GetLinks("");
        }

        public List<string> GetLinks(string rootUrl)
        {
            var ms = GetMatches(@"\<a[^\<\>]+?[hH][Rr][Ee][fF][\s]?=[\s""]?(?<href>[^""\<\>\s#]+)[^\<\>]+?\>");
            return (from Match item in ms select item.Groups["href"].Value into url where url.IndexOf("javascript:", StringComparison.Ordinal) < 0 select new Uri(new Uri(rootUrl), url) into uri select uri.ToString()).ToList();
        }

        public List<string> GetHref()
        {
            var ms = GetMatches(@"\<[^\<\>]+?[hH][Rr][Ee][fF][\s]?=[\s""]?(?<href>[^""\<\>\s]+)[^\<\>]+?\>");
            return (from Match item in ms select item.Groups["href"].Value).ToList();
        }

        public List<string> GetSrc()
        {
            var ms = GetMatches(@"\<[^\<\>]+?[sS][Rr][Cc][\s]?=[\s""]?(?<src>[^""\<\>\s]+)[^\<\>]+?\>");
            return (from Match item in ms select item.Groups["src"].Value).ToList();
        }
        
        public MatchCollection GetMatches(string pattern)
        {
            return Regex.Matches(_html, pattern);
        }

        public Match GetMatch(string pattern)
        {
            return Regex.Match(_html, pattern);
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="replace">自定义的替换</param>
        /// <returns></returns>
        public string GetText(string replace = null)
        {  
            _html = ReplaceHtml(_html);
            //替换掉 < 和 > 标记
            //_html = _html.Replace("<", "");
            //_html = _html.Replace(">", "");
            if (!string.IsNullOrWhiteSpace(replace))
            {
                Replace(replace);
            }
            // 替换被转义的
            _html = _html.Replace("\\n", "\n");
            _html = _html.Replace("\\xa1", "\xa1");
            _html = _html.Replace("\\xa2", "\xa2");
            _html = _html.Replace("\\xa3", "\xa3");
            _html = _html.Replace("\\xa9", "\xa9");
            //返回去掉_html标记的字符串
            return _html;
        }

        public bool Comparer(string url, string url2)
        {
            if (url.IndexOf("//", StringComparison.Ordinal) < 0 || url2.IndexOf("//", StringComparison.Ordinal) < 0)
            {
                return true;
            }
            var regex = new Regex(@"//[^/]+");
            return regex.Match(url).Value.Equals(regex.Match(url2).Value, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
