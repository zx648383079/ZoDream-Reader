/*
 * 静态数据
 * 方便运行时随时使用
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.Win32;

namespace 在线阅读
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
        /// 储存主页的网址
        /// </summary>
        public static string Url;
        /// <summary>
        /// 储存当前阅读的进度
        /// </summary>
        public static int index;
    }
}
