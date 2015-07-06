/*
 * 读取网页 
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 在线阅读.Models;

namespace 在线阅读.Class
{
    /// <summary>
    /// 读取网页内容
    /// </summary>
    public class HtmlReader:Reader
    {
        /// <summary>
        /// 构造函数，传入网址
        /// </summary>
        /// <param name="url"></param>
        public HtmlReader(string url):base(url)
        {
        }

        /// <summary>
        /// 载入列表
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public override List<Book> Loading(string regex)
        {
            string html = GetHtmlContent();

            MatchCollection ms = Regex.Matches(html, regex);
            return (from Match match in ms select new Book(match.Groups[2].Value, GetUrl(match.Groups[1].Value, _fileName))).ToList();
        }
        /// <summary>
        /// 获取正文
        /// </summary>
        /// <param name="regexs">正文正则，和替换正则</param>
        /// <returns></returns>
        public override string GetContent(string[] regexs)
        {
            if (regexs.Length !=2)
            {
                return "0";
            }

            string html = GetHtmlContent();
            //匹配正文
            Match m = Regex.Match(html, regexs[0]);
            string content = m.Groups[1].Value;

            MatchCollection ms = Regex.Matches(regexs[1], @"(?<a>.*?)替换(?<b>.*?)的值");
            foreach (Match match in ms)
            {
                string tem = content;
                string rep = match.Groups[2].Value.Replace("\\n", "\n");           //解决换行符被转义
                content = Regex.Replace(tem, match.Groups[1].Value, rep);

            }

            return content.Replace("\\n", "\n");
            //string str = Regex.Replace(content, @"\<script\>[\s\S]*?\</script\>", "");
            //string tem = Regex.Replace(str, @"\<[\s\S]*?\>", "\n"); //把所有的html标记替换为换行
            //string replace = tem.Replace("&nbsp;", " ");               //把所有的html空格标记还原
            //return Regex.Replace(replace, @"\s{4,}", "\n\n    ");         //把所有的四个及以上空白字符替换换行加四个空格；
        }

        /// <summary>
        /// 取网址
        /// </summary>
        /// <param name="newUrl">新网址</param>
        /// <param name="oldUrl">老网址，匹配域名</param>
        /// <returns></returns>
        private string GetUrl(string newUrl, string oldUrl)
        {
            if (newUrl == "")
            {
                return "";
            }
            string returnurl = newUrl;

            if (newUrl.LastIndexOf("http://", StringComparison.Ordinal) < 0)
            {
                if (newUrl.IndexOf("/", System.StringComparison.Ordinal) >= 0)
                {
                    string[] tem = newUrl.Split('/');
                    newUrl = tem[tem.Length - 1];
                }
                returnurl = oldUrl.Substring(0, oldUrl.LastIndexOf("/", StringComparison.Ordinal) + 1) + newUrl;
            }

            return returnurl;
        }
        /// <summary/>
        /// 获取源代码
        /// <returns></returns>
        public string GetHtmlContent()
        {

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(_fileName);
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress),GetEncoding(response));
                    else
                        reader = new StreamReader(response.GetResponseStream(), GetEncoding(response));
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();
                if (request != null)
                    request = null;
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取HTML网页的编码
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private Encoding GetEncoding(HttpWebResponse response)
        {
            StreamReader reader;

            if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
            else
                reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
            string html = reader.ReadToEnd();
            Regex reg_charset = new Regex(@"charset\b\s*=\s*(?<charset>[^""]*)");
            if (reg_charset.IsMatch(html))
            {

                return Encoding.GetEncoding(reg_charset.Match(html).Groups["charset"].Value);
            }
            else if (response.CharacterSet != String.Empty)
            {

                return Encoding.GetEncoding(response.CharacterSet);
            }
            else
                return Encoding.Default;
        }
    }
}
