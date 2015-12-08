/*
 * 自定义方法
 * 只是暂时的使用
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// 获取文件类型
        /// </summary>
        /// <param name="file">地址</param>
        /// <returns></returns>
        public bool GetKind(string file)
        {
            bool kind = Regex.IsMatch(file, @"^[a-zA-Z]:");
            return kind;
        }

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
            List<BookRegex> bookRegexs = new List<BookRegex>();
            string fileName = GetFileName("Regex.list");

            if (!File.Exists(fileName)) return bookRegexs;
            string[] regex= File.ReadAllLines(fileName, Encoding.UTF8);
            //bookRegex.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.+?)小说(?<d>.+)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value,m.Groups[4].Value)));

            bookRegexs.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.*?)小说(?<d>.*)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value)));

            return bookRegexs;
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
                if (!GetKind(url))
                {
                    return Regex.Match(url, @"[\w\.]+\w*\.\w*").Value;
                }
                else
                {
                    return "本地";
                }
                
            }
            else
            {
                return "";
            }
            
        }

        #region 多线程下载文件
        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="index">开始位置</param>
        /// <param name="count">个数</param>
        public void DownBook(int index, int count)
        {
            if (Model.Kind) return;

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
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }

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
            HtmlReader htmlReader = new HtmlReader();
            StreamWriter writer = new StreamWriter(_fileName, true);

            for (int i = 0; i < _count; i++)
            {
                Book book =Model.Books[_index + i];
                writer.WriteLine(book.Name);
                string content = htmlReader.GetContent(book.Url,Model.NowBookRegex.Content,Model.NowBookRegex.Replace);
                writer.Write(content + "\n");
            }
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
        /// <summary>
        /// 载入章节
        /// </summary>
        public void GetChapter()
        {
            BookModel model = Model.BookModels[0];
            if (Model.Kind)
            {
                TxtReader reader = new TxtReader();
                Model.Books = reader.Loading(model.FileName, Model.NowBookRegex.Chapter);
            }
            else
            {
                HtmlReader reader = new HtmlReader();
                Model.Books = reader.Loading(model.FileName, Model.NowBookRegex.Chapter);
            }
        }
        /// <summary>
        /// 获取正文
        /// </summary>
        /// <returns></returns>
        public string GetContent()
        {
            string content;
            BookModel model = Model.BookModels[0];
            if (Model.Kind)
            {
                string nextChaper = model.Index ==Model.Books.Count -1 ? "" : Model.Books[model.Index + 1].Name;

                TxtReader reader = new TxtReader();
                content = reader.GetText(model.FileName,Model.Books[model.Index].Name,nextChaper);
            }
            else
            {
                HtmlReader reader = new HtmlReader();
                content = reader.GetContent(Model.Books[model.Index].Url, Model.NowBookRegex.Content, Model.NowBookRegex.Replace);
            }
            return content;
        }

    }
}
