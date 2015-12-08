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
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using 在线阅读.Models;

namespace 在线阅读.Class
{
    public class UserMethod
    {
        /// <summary>
        /// 加载正在读的书
        /// </summary>
        /// <returns></returns>
        public List<BookModel> LoadBook()
        {
            MatchCollection ms = Regex.Matches(GetFileTxt(GetFileName("BookModel.ini")),
                @"(?<name>.*?)小说(?<url>.*?)小说(?<index>.*)");

            return (from Match item in ms select new BookModel(item.Groups[1].Value, item.Groups[2].Value, int.Parse(item.Groups[3].Value))).ToList();
        }

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bookModel"></param>
        public void SavaBook(List<BookModel> bookModel)
        {
           string fileName= GetFileName("BookModel.ini");
            int count = bookModel.Count;
            string[] model = new string[count];
            for (int i = 0; i < count; i++)
            {
                BookModel book = bookModel[i];
                model[i] = book.Name + "小说" + book.FileName + "小说" + book.Index;
            }
            Sava(model, fileName);
        }
        #endregion

        /// <summary>
        /// 得到当前程序运行路径下的文件路径
        /// </summary>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        private string GetFileName(string name)
        {
            String appStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\"+name;
            return fileName;
        }
        /// <summary>
        /// 得到文件的内容
        /// </summary>
        /// <param name="fileName">路径</param>
        /// <returns></returns>
        private string GetFileTxt(string fileName)
        {
            string txt = "";
            if (File.Exists(fileName))
            {
                txt= File.ReadAllText(fileName,Encoding.UTF8);
            }
            return txt;
        }
        /// <summary>
        /// 保存的总方法
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fileName"></param>
        private void Sava(string[] list, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllLines(fileName, list, Encoding.UTF8);
        }

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
                regex[i] = book.Web + "小说" + book.Chapter + "小说" + book.Content + "小说" + book.Replace;
            }
            
            string fileName = GetFileName("Regex.list");

            Sava(regex, fileName);

        }
        /// <summary>
        /// 载入正则表达式
        /// </summary>
        /// <returns></returns>
        public List<BookRegex> LoadRegex()
        {
            List<BookRegex> bookRegex = new List<BookRegex>();
            string fileName = GetFileName("Regex.list");

            if (!File.Exists(fileName)) return bookRegex;
            string[] regex= File.ReadAllLines(fileName, Encoding.UTF8);
            bookRegex.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.+?)小说(?<d>.+)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value,m.Groups[4].Value)));

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
                if (newUrl.IndexOf("/", System.StringComparison.Ordinal)>=0)
                {
                    string[] tem = newUrl.Split('/');
                    newUrl = tem[tem.Length - 1];
                }
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
                if (Regex.IsMatch(url,"http"))
                {
                    return Regex.Match(url, @"[\w\.]+\w*\.\w*").Value;
                }
                else
                {
                    return "txt";
                }
                
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
        public void GetList()
        {
            Model.Books.Clear();
            GetRegex(LoadRegex());
            if (Regex.IsMatch(Model.BookModels[0].FileName, "http"))
            {
                string html = GetHtmlContent(Model.BookModels[0].FileName, GetEncoding(Model.BookModels[0].FileName));

                MatchCollection ms = Regex.Matches(html, Model.NowBookRegex.Chapter);
                foreach (Match match in ms)
                {
                    Model.Books.Add(new Book(match.Groups[2].Value, GetUrl(match.Groups[1].Value, Model.BookModels[0].FileName)));
                }

            }
            else
            {
                string txt = GetFileTxt(Model.BookModels[0].FileName);
                MatchCollection ms = Regex.Matches(txt,Model.NowBookRegex.Chapter);
            }
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

            MatchCollection ms = Regex.Matches(Model.NowBookRegex.Replace, @"(?<a>.*?)替换(?<b>.*?)的值");
            foreach (Match match in ms)
            {
                string tem = content;
                string rep = match.Groups[2].Value.Replace("\\n","\n");           //解决换行符被转义
                content  = Regex.Replace(tem, match.Groups[1].Value, rep);

            }

            return content.Replace("\\n","\n");
            //string str = Regex.Replace(content, @"\<script\>[\s\S]*?\</script\>", "");
            //string tem = Regex.Replace(str, @"\<[\s\S]*?\>", "\n"); //把所有的html标记替换为换行
            //string replace = tem.Replace("&nbsp;", " ");               //把所有的html空格标记还原
            //return Regex.Replace(replace, @"\s{4,}", "\n\n    ");         //把所有的四个及以上空白字符替换换行加四个空格；

        }
        #region 多线程下载文件
        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="index">开始位置</param>
        /// <param name="count">个数</param>
        public void DownBook(int index, int count)
        {
            SaveFileDialog savaFile = new SaveFileDialog { Filter = "文本文件|*.txt|所有文件|*.*", Title = "保存文本" };
            if (savaFile.ShowDialog() != true) return;

            //ThreadStart startDownload = new ThreadStart(DownLoad);//线程起始设置：即每个线程都执行DownLoad()
            //Thread[] downloadThread = new Thread[count];//为线程申请资源，确定线程总数
            //for (int i = 0; i < n; i++)//开启指定数量的线程数
            //{
            //    downloadThread[i] = new Thread(startDownload);//指定线程起始设置
            //    downloadThread[i].Start();//逐个开启线程
            //} 

            _index = index;
            _count = count;
            _fileName = savaFile.FileName;

            ThreadStart startDownload = new ThreadStart(DownLoad); //线程起始设置：即每个线程都执行DownLoad()，注意：DownLoad()必须为不带有参数的方法
            Thread downloadThread = new Thread(startDownload); //实例化要开启的新类
            downloadThread.Start();//开启线程

            
        }

        private int _index;
        private int _count;
        private string _fileName;


        /// <summary>
        /// 下载线程
        /// </summary>
        private void DownLoad()
        {
            StringBuilder bookContent = new StringBuilder();
            UserMethod method = new UserMethod();
            for (int i = 0; i < _count; i++)
            {
                Book book = Model.Books[_index + i];
                bookContent.Append(book.Name + "\n");
                string content = method.GetContent(book.Url);
                bookContent.Append( content+ "\n");
            }
            string[] tem = bookContent.ToString().Split('\n');
            File.WriteAllLines(_fileName, tem);
            MessageBox.Show("下载完成！","提示");
        }
        #endregion
        /// <summary>
        /// 取相关网站的正则表达式
        /// </summary>
        /// <param name="regexs"></param>
        public void GetRegex(List<BookRegex> regexs)
        {
            string web = GetWeb(Model.BookModels[0].FileName);
            foreach (BookRegex item in regexs)
            {
                if (web == item.Web)
                {
                    Model.NowBookRegex = item;
                    return;
                }
            }
        }
    }
}
