using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Reader.Model;
using ZoDream.Helper.Local;

namespace ZoDream.Reader.Helper
{
    public class LocalHelper
    {

        public static string GetSafeFile(string file)
        {
            return file.Replace("/", "");
        }

        public static List<ChapterItem> GetChapters(string file)
        {
            var pattern = @"^[\s]{0,6}第?[\s]*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?[\s\S]{0,20}$";
            var fs = new FileStream(file, FileMode.Open);
            var txtEncoding = new TxtEncoder().GetEncoding(fs);            //获取编码
            var bookChapters = new List<ChapterItem>();
            var reader = new StreamReader(fs, txtEncoding);
            var line = reader.ReadLine();
            var content = new StringBuilder();
            var name = DateTime.Now.ToFileTime().ToString();
            bookChapters.Add(new ChapterItem(line, name));
            while ((line = reader.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, pattern))
                {
                    WriteTemp(content.ToString(), name);
                    content.Clear();
                    name = DateTime.Now.ToFileTime().ToString();
                    bookChapters.Add(new ChapterItem(line, name));
                }
                else
                {
                    content.AppendLine(line);
                }
            }
            WriteTemp(content.ToString(), name);
            reader.Close();
            fs.Close();
            return bookChapters;
        }

        public static readonly string TempDir = AppDomain.CurrentDomain.BaseDirectory + "\\temp\\";

        public static void CreateTempDir()
        {
            Directory.CreateDirectory(TempDir);
        }

        public static void WriteTemp(string content, string name)
        {
            var fs = new FileStream(TempDir + name, FileMode.Create);
            var writer = new StreamWriter(fs, Encoding.UTF8);
            writer.Write(content);
            writer.Close();
        }

        public static string ReadTemp(string name)
        {
            var file = TempDir + name;
            var reader = new StreamReader(file, Encoding.UTF8);
            var content = reader.ReadToEnd();
            reader.Close();
            //File.Delete(file);
            return content;
        }

        public static void WriteLog(string message)
        {
            var fs = new FileStream(TempDir + "log.txt", FileMode.Append);
            var writer = new StreamWriter(fs, Encoding.UTF8);
            writer.WriteLine(message);
            writer.Close();
        }
    }
}
