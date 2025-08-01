﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Storage
{
    public static class LocationStorage
    {
        /// <summary>
        /// 读文本文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> ReadAsync(string file)
        {
            if (!File.Exists(file))
            {
                return string.Empty;
            }
            using var reader = Reader(file);
            var content = await reader.ReadToEndAsync();
            return content;
        }

        public static StreamReader Reader(string file)
        {
            var fs = File.OpenRead(file);
            return Reader(fs);
        }

        public static StreamReader Reader(Stream input)
        {
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding("gb2312");
            }
            catch (ArgumentException)
            {
                encoding = Encoding.UTF8;
            }
            return new StreamReader(input, TxtEncoder.GetEncoding(input, encoding));
        }

        /// <summary>
        /// 写文本文件 默认使用无 bom 的UTF8编码
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        public static async Task WriteAsync(string file, string content)
        {
            await WriteAsync(file, content, new UTF8Encoding(false));
        }

        public static async Task WriteAsync(string file, string content, bool append)
        {
            await WriteAsync(file, content, new UTF8Encoding(false), append);
        }

        public static async Task WriteAsync(string file, string content, string encoding)
        {
            await WriteAsync(file, content, Encoding.GetEncoding(encoding));
        }

        public static async Task WriteAsync(string file, string content, Encoding encoding)
        {
            await WriteAsync(file, content, encoding, false);
        }

        public static async Task WriteAsync(string file, string content, Encoding encoding, bool append)
        {
            using var writer = new StreamWriter(file, append, encoding);
            await writer.WriteAsync(content);
        }

        public static StreamWriter Writer(string file)
        {
            return Writer(file, false);
        }

        public static StreamWriter Writer(string file, bool append)
        {
            FileStream fs;
            if (!append)
            {
                fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
                return new StreamWriter(fs, Encoding.UTF8);
            }
            fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var encoding = TxtEncoder.GetEncoding(fs);
            fs.Seek(0, SeekOrigin.End);
            return new StreamWriter(fs, encoding);
        }
    }
}
