using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Model
{
    public class ChapterItem
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Content { get; set; }

        public int BookId { get; set; }

        public string Url { get; set; }

        public ChapterItem()
        {
            
        }

        /// <summary>
        /// 本地文本用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public ChapterItem(string name, string file)
        {
            Name = name;
            Content = file;
        }

        /// <summary>
        /// 多线程下载时用
        /// </summary>
        /// <param name="name">章节名</param>
        /// <param name="url">网址</param>
        /// <param name="file">缓存路径</param>
        public ChapterItem(string name, string url, string file)
        {
            Name = name;
            Url = url;
            Content = file;
        }

        public ChapterItem(string name, string content, string url, int book)
        {
            Name = name;
            Url = url;
            Content = content;
            BookId = book;
        }

        public ChapterItem(IDataRecord reader)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
            Url = reader.GetString(2);
        }
    }
}
