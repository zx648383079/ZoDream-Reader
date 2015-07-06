using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 在线阅读.Models;

namespace 在线阅读.Class
{

    /// <summary>
    /// 抽象阅读
    /// </summary>
    public abstract class Reader
    {
        /// <summary>
        /// 路径或网址
        /// </summary>
        protected string _fileName;
        /// <summary>
        /// 构造函数传路径或网址
        /// </summary>
        /// <param name="fileName"></param>
        public Reader(string fileName)
        {
            this._fileName = fileName;
        }

        /// <summary>
        /// 载入目录
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        public abstract List<Book> Loading(string regex);
        /// <summary>
        /// 得到一章的内容
        /// </summary>
        /// <param name="regexs"></param>
        /// <returns></returns>
        public abstract string GetContent(string[] regexs);
    }
}
