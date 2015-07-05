/*
 * 静态数据
 * 方便运行时随时使用
 * 
 */

using System.Collections.Generic;
using 在线阅读.Models;

namespace 在线阅读.Class
{
    public static class Model
    {
        /// <summary>
        /// 存储正则表达式
        /// </summary>
        public static BookRegex NowBookRegex;
        /// <summary>
        /// 储存列表
        /// </summary>
        public static List<Book> Books = new List<Book>();

        /// <summary>
        /// 存储现在的书的数据
        /// </summary>
        public static List<BookModel> BookModels;
        /// <summary>
        /// 是不是本地文件，true是本地文件
        /// </summary>
        public static bool Kind;
    }
}
