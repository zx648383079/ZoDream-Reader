/*
 * 正在读的书及进度
 */

namespace 在线阅读.Models
{
    /// <summary>
    /// 书的具体信息
    /// </summary>
    public class BookModel
    {
        /// <summary>
        /// 书的名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路径或网址
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 正在读的书
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        public BookModel(string name,string fileName,int index)
        {
            this.Name = name;
            this.FileName = fileName;
            this.Index = index;
        }

    }
}
