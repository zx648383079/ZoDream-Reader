/*
 * 自定义方法
 * 只是暂时的使用
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Win32;

namespace 在线阅读
{
    public class UserMethod
    {
        /// <summary>
        /// 将blend的8位颜色值转为color  
        /// </summary>  
        /// <param name="color"></param>  
        /// <returns></returns>  
        public Color ToColor(string color)
        {
            Color returnColor = Colors.Gold;

            if (color == "") return returnColor;
            MatchCollection ms = Regex.Matches(color, @"\d+");
            switch (ms.Count)
            {
                case 3:
                    returnColor = Color.FromRgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value));
                    break;
                case 4:
                    returnColor = Color.FromArgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value), Convert.ToByte(ms[3].Value));
                    break;
            }
            return returnColor;
        }
        /// <summary>
        /// 保存正则法则
        /// </summary>
        /// <param name="bookRegex"></param>
        public void SavaRegex(List<BookRegex> bookRegex)
        {
            int count = bookRegex.Count;
            string[] regex = new string[count];
            for (int i = 0; i <count  ; i++)
            {
                BookRegex book = bookRegex[i];
                regex[i] = book.Web + "小说" + book.Name + "小说" + book.Chapter + "小说" + book.Content;
            }
            String appStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\Regex.list";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllLines(fileName, regex, Encoding.UTF8);

        }
        /// <summary>
        /// 载入正则表达式
        /// </summary>
        /// <returns></returns>
        public List<BookRegex> LoadRegex()
        {
            List<BookRegex> bookRegex = new List<BookRegex>();
            String appStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\Regex.list";

            if (!File.Exists(fileName)) return bookRegex;
            string[] regex= File.ReadAllLines(fileName, Encoding.UTF8);
            bookRegex.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.+?)小说(?<d>.+)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value)));

            return bookRegex;
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
                returnurl = oldUrl.Substring(0, oldUrl.LastIndexOf("/", StringComparison.Ordinal) + 1) + newUrl;
            }

            return returnurl;
        }
        /// <summary>
        /// 提取网址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetWeb(string url)
        {
            if (url!=null)
            {
                return Regex.Match(url, @"\w*\.\w*").Value;
            }
            else
            {
                return "";
            }
            
        }


        /// <summary/>
        /// 获取源代码
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetHtmlContent(string url, string encoding)
        {
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
        /// <summary>
        /// 获取HTML网页的编码
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetEncoding(string url)
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


        /// <summary>
        /// 得到目录
        /// </summary>
        public string GetList()
        {
            Model.Books.Clear();
            string html = GetHtmlContent(Model.Url, GetEncoding(Model.Url));

            Match m = Regex.Match(html, Model.NowBookRegex.Name);

            string name = m.Groups[1].Value;


            MatchCollection ms = Regex.Matches(html, Model.NowBookRegex.Chapter);
            foreach (Match match in ms)
            {
                Model.Books.Add(new Book(match.Groups[2].Value, GetUrl(match.Groups[1].Value, Model.Url)));
            }

            return name;
        }

        /// <summary>
        /// 取正文
        /// </summary>
        /// <param name="url"></param>
        public string GetContent(string url)
        {
            string html = GetHtmlContent(url, GetEncoding(url));
            //匹配正文
            Match m = Regex.Match(html, Model.NowBookRegex.Content);
            string content = m.Groups[1].Value;
            string tem = Regex.Replace(content, @"\<[\s\S]*?\>", "\n");
            return tem.Replace(@"&nbsp;", "  ");

        }

        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="index">开始位置</param>
        /// <param name="count">个数</param>
        public void DownBook(int index, int count)
        {
            StringBuilder book = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                book.Append(Model.Books[index + i].Name + "\n\n");
                book.Append(GetContent(GetHtmlContent(Model.Books[index + i].Url, GetEncoding(Model.Books[index + i].Url))) + "\n\n");
            }

            SaveFileDialog savaFile = new SaveFileDialog { Filter = "文本文件|*.txt|所有文件|*.*", Title = "保存文本" };
            if (savaFile.ShowDialog() != true) return;
            string[] tem = book.ToString().Split('\n');
            File.WriteAllLines(savaFile.FileName, tem);
        }
        /// <summary>
        /// 取相关网站的正则表达式
        /// </summary>
        /// <param name="regexs"></param>
        public void GetRegex(List<BookRegex> regexs)
        {
            string web = GetWeb(Model.Url);
            foreach (BookRegex item in regexs.Where(item => web == GetWeb(item.Web)))
            {
                Model.NowBookRegex = item;
                return;
            }
        }

    }
}
