/*
 * 读取本地文件
 * 
 */

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using 在线阅读.Models;

namespace 在线阅读.Class
{
    /// <summary>
    /// 本地文本文件操作
    /// </summary>
    public class TxtReader
    {
        /// <summary>
        /// 导入文件目录
        /// </summary>
        /// <param name="file">路径</param>
        /// <param name="regex">目录的正则</param>
        /// <returns>目录列表</returns>
        public List<Book> Loading(string file,string regex)
        {
            TxtEncoder txtEncoder = new TxtEncoder();
            Encoding txtEncoding = txtEncoder.GetEncoding(file);
            List<Book> bookChapter = new List<Book>();
            
            StreamReader reader = new StreamReader(file,txtEncoding);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, regex))
                {
                    bookChapter.Add(new Book(line,""));
                }
            }

            reader.Close();
            return bookChapter;
        }
        /// <summary>
        /// 载入一章的内容
        /// </summary>
        /// <param name="file">路径</param>
        /// <param name="start">当前章节</param>
        /// <param name="end">下一章节</param>
        /// <returns></returns>
        public string GetText(string file,string start,string end)
        {
            TxtEncoder txtEncoder = new TxtEncoder();
            Encoding txtEncoding = txtEncoder.GetEncoding(file);            //获取编码

            StreamReader reader = new StreamReader(file, txtEncoding);
            string line;

            StringBuilder tem = new StringBuilder();
            //读到当前章节停止循环
            while ((line = reader.ReadLine()) != null)
            {
                if (line ==start)
                {
                    break;
                }
            }

            //提取内容，直到下一章节停止
            while ((line = reader.ReadLine()) != null)
            {
                if (line ==end)
                {
                    break;
                }
                tem.AppendLine(line);
            }


            reader.Close();

            return tem.ToString();
        }
    }
}
