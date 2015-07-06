/*
 * 读取本地文件
 * 
 */

using System;
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
    public class TxtReader:Reader
    {
        /// <summary>
        /// 构造函数传路径
        /// </summary>
        /// <param name="file"></param>
        public TxtReader(string file):base(file)
        {

        }

        /// <summary>
        /// 导入文件目录
        /// </summary>
        /// <param name="regex">目录的正则</param>
        /// <returns>目录列表</returns>
        public override List<Book> Loading(string regex)
        {
            FileStream fs = new FileStream(_fileName, FileMode.Open);
            Encoding txtEncoding = GetEncoding(fs);            //获取编码
            List<Book> bookChapter = new List<Book>();
            
            StreamReader reader = new StreamReader(fs,txtEncoding);
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
        /// <returns></returns>
        public override string GetContent(string[] content)
        {
            if (content.Length !=2)
            {
                return "0";
            }

            FileStream fs = new FileStream(_fileName, FileMode.Open);
            Encoding txtEncoding = GetEncoding(fs);            //获取编码

            StreamReader reader = new StreamReader(fs, txtEncoding);
            string line;

            StringBuilder tem = new StringBuilder();
            //读到当前章节停止循环
            while ((line = reader.ReadLine()) != null)
            {
                if (line ==content[0])
                {
                    break;
                }
            }

            //提取内容，直到下一章节停止
            while ((line = reader.ReadLine()) != null)
            {
                if (line ==content[1])
                {
                    break;
                }
                tem.AppendLine(line);
            }


            reader.Close();

            return tem.ToString();
        }

        /// <summary>   
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>
        /// <returns></returns>   
        public Encoding GetEncoding(FileStream stream)
        {
            Encoding targetEncoding = Encoding.Default;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe   
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode   
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8   
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置         
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
    }
}
