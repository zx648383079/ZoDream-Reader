using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using ZoDream.Reader.Helper.Local;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.Helper
{
    public class LocalHelper
    {

        /// <summary>
        /// 浏览文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExploreFile(string filePath)
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "explorer",
                    Arguments = @"/select," + filePath
                }
            };
            //打开资源管理器
            //选中"notepad.exe"这个程序,即记事本
            proc.Start();
        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void ExplorePath(string path)
        {
            Process.Start("explorer.exe", path);
        }


        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static List<string> GetAllFile(string dir)
        {
            var files = new List<string>();
            if (string.IsNullOrWhiteSpace(dir))
            {
                return files;
            }
            var theFolder = new DirectoryInfo(dir);
            var dirInfo = theFolder.GetDirectories();
            //遍历文件夹
            foreach (var nextFolder in dirInfo)
            {
                files.AddRange(GetAllFile(nextFolder.FullName));
            }

            var fileInfo = theFolder.GetFiles();
            //遍历文件
            files.AddRange(fileInfo.Select(nextFile => nextFile.FullName));
            return files;
        }

        public static string ChooseSaveFile(string name = "", string filter = "文本文件|*.txt|所有文件|*.*")
        {
            var open = new SaveFileDialog
            {
                Title = "选择保存路径",
                Filter = filter,
                FileName = name
            };
            return open.ShowDialog() == true ? open.FileName : null;
        }

        /// <summary>
        /// 选择个文件
        /// </summary>
        /// <returns></returns>
        public static string ChooseFile(string filter = "文本文件|*.txt|所有文件|*.*")
        {
            var open = new OpenFileDialog
            {
                Multiselect = true,
                Filter = filter,
                Title = "选择文件"
            };
            return open.ShowDialog() == true ? open.FileName : string.Empty;
        }

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
            fs.Close();
        }

        public static string ReadTemp(string name)
        {
            var file = TempDir + name;
            var reader = new StreamReader(file, Encoding.UTF8);
            var content = reader.ReadToEnd();
            reader.Close();
            File.Delete(file);
            return content;
        }

        public static bool OpenBrowser(string url)
        {
            var key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            if (key == null) return false;
            var s = key.GetValue("").ToString();
            string browserpath = null;
            browserpath = s.StartsWith("\"") ? s.Substring(1, s.IndexOf('\"', 1) - 1) : s.Substring(0, s.IndexOf(" ", StringComparison.Ordinal));
            return Process.Start(browserpath, url) != null;
        }
    }
}
