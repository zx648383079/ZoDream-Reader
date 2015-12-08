/*
 * 章节名及网址
 */

namespace 在线阅读.Models
{
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

         public Book(string name,string url)
         {
             this.Name = name;
             this.Url = url;
         }
    }
}
