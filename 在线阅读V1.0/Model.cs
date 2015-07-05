using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace 在线阅读
{
    public static class Model
    {
        /// <summary>
        /// 标题的正则表达式
        /// </summary>
        public static string TitleRegex;
        /// <summary>
        /// 正文的正则表达式
        /// </summary>
        public static string ContentRegex;
        /// <summary>
        /// 上一章网址的正则表达式
        /// </summary>
        public static string BeforeRegex;
        /// <summary>
        /// 下一章网址的正则表达式
        /// </summary>
        public static string NextRegex;
        /// <summary>
        /// 标题
        /// </summary>
        public static string Title;
        /// <summary>
        /// 正文
        /// </summary>
        public static string Content;
        /// <summary>
        /// 字体大小
        /// </summary>
        public static int FontSize;
        /// <summary>
        /// 字体
        /// </summary>
        public static string Font;
        /// <summary>
        /// 当前网址
        /// </summary>
        public static string Url;
        /// <summary>
        /// 上一章网址的正则表达式
        /// </summary>
        public static string BeforeUrl;
        /// <summary>
        /// 下一章网址的正则表达式
        /// </summary>
        public static string NextUrl;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public static string FontColor;
        /// <summary>
        /// 背景
        /// </summary>
        public static string Background;
        /// <summary>
        /// 下载网页源码进行读取
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public static bool Reader(string url)
        {
            Url = url;
            try
            {
                string html = GetHtmlContent(url, GetEncoding(url));
                //匹配标题
                Match ti = Regex.Match(html, TitleRegex);
                Title = ti.Groups[1].Value;
                //匹配正文
                Match content = Regex.Match(html,ContentRegex);
                GetContent(content.Groups[1].Value);

               //匹配上一章网址
                Match be = Regex.Match(html, BeforeRegex);
                BeforeUrl =GetUrl(be.Groups[1].Value);
                //匹配下一章网址
                Match ne = Regex.Match(html, NextRegex);
                NextUrl =GetUrl(ne.Groups[1].Value);


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 取正文
        /// </summary>
        /// <param name="content"></param>
        private static void GetContent(string content)
        {
            string tem= Regex.Replace(content, @"\<[\s\S]*?\>", "\n");
            Content = tem.Replace(@"&nbsp;", "  ");
        }
        /// <summary>
        /// 取网址
        /// </summary>
        /// <param name="newurl"></param>
        /// <returns></returns>
        private static string GetUrl(string newurl)
        {
            if (newurl =="")
            {
                return "";
            }
            string returnurl = newurl;

            if (newurl.LastIndexOf("http://", System.StringComparison.Ordinal)<0)
            {
                returnurl = Url.Substring(0, Url.LastIndexOf("/", System.StringComparison.Ordinal) + 1) + newurl;
            }

            return returnurl;
        }
        /// <summary>
        /// 载入设置内容
        /// </summary>
        public static void OpenSet()
        {
            String appStartupPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\Set.ini";
            if (File.Exists(fileName))
            {
                string[] tem= File.ReadAllLines(fileName, Encoding.UTF8);
                TitleRegex = tem[0];
                ContentRegex = tem[1];
                BeforeRegex = tem[2];
                NextRegex = tem[3];
                Url=tem[4] ;
                Font=tem[5] ;
                FontSize = int.Parse(tem[6]);
                FontColor = tem[7];
                Background = tem[8];
            }
            else
            {
                Font = "宋体";
                FontSize = 16;
                FontColor = "0,0,0";
                Background = "255,90,247,90";
            }
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        public static void SavaSet()
        {
            String appStartupPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\Set.ini";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            string[] tem = new string[9];
            tem[0]=TitleRegex;
            tem[1] =ContentRegex;
            tem[2]=BeforeRegex;
            tem[3]= NextRegex;
            tem[4] = Url;
            tem[5] = Font;
            tem[6] = FontSize.ToString(CultureInfo.InvariantCulture);
            tem[7] = FontColor;
            tem[8] = Background;
            File.WriteAllLines(fileName,tem, Encoding.UTF8);
        }

        /// <summary>
        /// 获取源代码
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlContent(string url, string encoding)
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
            return string.Empty;
        }
        /// <summary>
        /// 获取HTML网页的编码
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetEncoding(string url)
        {
            string charset = string.Empty;
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
                    else if (response.CharacterSet != string.Empty)
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
        /// 将blend的8位颜色值转为color  
        /// </summary>  
        /// <param name="color"></param>  
        /// <returns></returns>  
        public static Color ToColor(string color)
        {
            Color returnColor=Colors.Gold;

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
    }
}
