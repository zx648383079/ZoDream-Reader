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
    public class HtmlReader
    {
        /// <summary>
        /// 载入列表
        /// </summary>
        /// <param name="url"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public List<Book> Loading(string url,string regex)
        {
            string html = GetHtmlContent(url);

            MatchCollection ms = Regex.Matches(html, regex);
            return (from Match match in ms select new Book(match.Groups[2].Value, GetUrl(match.Groups[1].Value, url))).ToList();
        }
        /// <summary>
        /// 获取正文
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="regex">正文正则</param>
        /// <param name="replace">正文替换的正则集合</param>
        /// <returns></returns>
        public string GetContent(string url, string regex, string replace)
        {
            string html = GetHtmlContent(url);
            //匹配正文
            Match m = Regex.Match(html, regex);
            string content = m.Groups[1].Value;

            MatchCollection ms = Regex.Matches(replace, @"(?<a>.*?)替换(?<b>.*?)的值");
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
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetHtmlContent(string url)
        {
            string encoding = GetEncoding(url);

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.GetEncoding(encoding));
                    else
                        reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
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
        /// <summary/>
        /// 获取HTML网页的编码
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetEncoding(string url)
        {
            string charset = String.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    else
                        reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                    string html = reader.ReadToEnd();
                    Regex reg_charset = new Regex(@"charset\b\s*=\s*(?<charset>[^""]*)");
                    if (reg_charset.IsMatch(html))
                    {
                        return reg_charset.Match(html).Groups["charset"].Value;
                    }
                    else if (response.CharacterSet != String.Empty)
                    {
                        return response.CharacterSet;
                    }
                    else
                        return Encoding.Default.BodyName;
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
            return Encoding.Default.BodyName;
        }
    }
}
