/*
 * 章节名及网址
 */

namespace 在线阅读.Models
{
    /// <summary>
    /// 书的信息
    /// </summary>
     public class Book
    {
         /// <summary>
         /// 章节名
         /// </summary>
         public string Name { get; set; }
         /// <summary>
         /// 网址
         /// </summary>
         public string Url { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
         public Book(string name,string url)
         {
             this.Name = name;
             this.Url = url;
         }
    }
}
