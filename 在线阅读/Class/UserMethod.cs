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
    /// <summary>
    /// 总方法
    /// </summary>
    public class UserMethod
    {
        /// <summary>
        /// 加载正在读的书
        /// </summary>
        /// <returns></returns>
        public List<BookModel> LoadBook()
        {
            if (!File.Exists(GetFile()))
            {
                const string setini = "Body{\nFont=方正启体简体;FontSize=30;FontColor=0,0,0;Background=255,90,247,90\n}\nRegex{\n" +
                                      @"本地小说^[\w].*小说小说" + "\n}\nModel{\n}";
                File.WriteAllText(GetFile(), TextTOTxt(setini), Encoding.UTF8);
            }

            TxtReader reader = new TxtReader(GetFile());
            string model = reader.GetContent(new string[] { "Model{", "}" });

            MatchCollection ms = Regex.Matches(model,
                @"(?<name>.*?)小说(?<url>.*?)小说(?<index>.*)");

            return (from Match item in ms select new BookModel(item.Groups[1].Value, item.Groups[2].Value, int.Parse(item.Groups[3].Value))).ToList();
        }
        /// <summary>
        /// 字体等
        /// </summary>
        /// <returns></returns>
        public string[] LoadBody()
        {
            TxtReader reader = new TxtReader(GetFile());
            string body = reader.GetContent(new string[] { "Body{", "}" });
            Match m = Regex.Match(body,
                @"Font=(?<font>.*?);FontSize=(?<size>\d*?);FontColor=(?<color>.*?);Background=(?<bg>.*)");
            string[] model = new string[4];
            for (int i = 0; i < 4; i++)
            {
                model[i] = m.Groups[i + 1].Value;
            }
            return model;
        }
        /// <summary>
        /// 保存书
        /// </summary>
        /// <param name="body"></param>
        public void SavaBody(string[] body)
        {
            TxtReader reader = new TxtReader(GetFile());
            string oldbody = "Body{\n" + reader.GetContent( new string[] { "Body{", "}" }) + "}";

            string newBody = "Body{\nFont="+body[0]+";FontSize="+body[1]+";FontColor="+body[2]+";Background="+body[3]+ "\n}";

            Sava(newBody, oldbody);

        }

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bookModel"></param>
        public void SavaBook(List<BookModel> bookModel)
        {
            TxtReader reader = new TxtReader(GetFile());
            string oldmodel = "Model{\n" + reader.GetContent(new string[] { "Model{", "}" }) + "}";



            int count = bookModel.Count;
            StringBuilder model = new StringBuilder();
            model.Append("Model{\n");

            for (int i = 0; i < count; i++)
            {
                BookModel book = bookModel[i];
                model.Append(book.Name + "小说" + book.FileName + "小说" + book.Index+"\n");
            }

            model.Append("}");

            Sava(model.ToString(),oldmodel);
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

        private string GetFile()
        {
            String appStartupPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string fileName = appStartupPath + "\\Set.ini";
            return fileName;

        }

        /// <returns></returns>
        /// <summary>
        /// 保存的总方法
        /// </summary>
        /// <param name="newSet">新的设置</param>
        /// <param name="oldSet">旧的设置</param>
        private void Sava(string newSet,string oldSet)
        {
            string file = GetFile();
            

            StreamReader reader = new StreamReader(file, Encoding.UTF8);

            string setini =TextTOTxt( reader.ReadToEnd());

            reader.Close();

            string tem1 = TextTOTxt(newSet);
            string tem2 = TextTOTxt(oldSet);

            string tem = setini.Replace(tem2,tem1);

            StreamWriter writer = new StreamWriter(file, false,Encoding.UTF8);
            writer.Write(tem);
            writer.Close();

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
            TxtReader reader = new TxtReader(GetFile());
            string oldRegex = "Regex{\n" + reader.GetContent(new string[] { "Regex{", "}" }) + "}";

            StringBuilder regex = new StringBuilder();
            regex.Append("Regex{\n");

            int count = bookRegex.Count;
            for (int i = 0; i <count  ; i++)
            {
                BookRegex book = bookRegex[i];
                regex.Append(book.Web + "小说" + book.Chapter + "小说" + book.Content + "小说" + book.Replace+"\n");
            }

            regex.Append("}");

            Sava(regex.ToString(), oldRegex);

        }
        /// <summary>
        /// 载入正则表达式
        /// </summary>
        /// <returns></returns>
        public List<BookRegex> LoadRegex()
        {
            string fileName = GetFile();

            TxtReader reader = new TxtReader(GetFile());
            string regex =TxtToText( reader.GetContent(new string[] { "Regex{", "}" }));

            

            MatchCollection ms = Regex.Matches(regex,
                @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.*?)小说(?<d>.*)");


            //bookRegex.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.+?)小说(?<d>.+)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value,m.Groups[4].Value)));

            //bookRegexs.AddRange(regex.Select(item => Regex.Match(item, @"(?<a>.+?)小说(?<b>.+?)小说(?<c>.*?)小说(?<d>.*)")).Select(m => new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value)));

            return (from Match m in ms select new BookRegex(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value)).ToList();
        }
        /// <summary>
        /// 保存时把\n转换为\r\n
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string TextTOTxt(string text)
        {
            return Regex.Replace(text, @"[\r\n]+", "\r\n"); ;
        }
        /// <summary>
        /// 把txt带的\r替换为空
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string TxtToText(string txt)
        {
            return Regex.Replace(txt, "\r","");
        }
        
        /// <summary>
        /// 提取网址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetWeb(string url)
        {
            string web = "";
            if (url == null) return web;
            if (GetKind(url))
            {
                web = "本地";
            }
            else
            {
                web = Regex.IsMatch(url, @"[\w\.]+\w*\.\w*") ? Regex.Match(url, @"[\w\.]+\w*\.\w*").Value : "本地";
                    
            }


            return web;
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
            
            StreamWriter writer = new StreamWriter(_fileName, true,Encoding.UTF8);

            for (int i = 0; i < _count; i++)
            {
                Book book =Model.Books[_index + i];
                HtmlReader htmlReader = new HtmlReader(book.Url);
                string content =book.Name+"\n"+htmlReader.GetContent(new string[] { Model.NowBookRegex.Content, Model.NowBookRegex.Replace })+"\n";
                writer.Write(Regex.Replace(content,"\r*\n","\r\n"));
            }

            writer.Close();
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
                TxtReader reader = new TxtReader(model.FileName);
                Model.Books = reader.Loading( Model.NowBookRegex.Chapter);
            }
            else
            {
                HtmlReader reader = new HtmlReader(model.FileName);
                Model.Books = reader.Loading(Model.NowBookRegex.Chapter);
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
                string nextChaper = model.Index == Model.Books.Count - 1 ? "" : Model.Books[model.Index + 1].Name;

                TxtReader reader = new TxtReader(model.FileName);
                content = reader.GetContent(new string[] { Model.Books[model.Index].Name, nextChaper });
            }
            else
            {
                HtmlReader reader = new HtmlReader(Model.Books[model.Index].Url);
                content = reader.GetContent(new string[] { Model.NowBookRegex.Content, Model.NowBookRegex.Replace });
            }
            return content;
        }

    }
}
