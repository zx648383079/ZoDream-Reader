/*
 * 章节名及网址
 */

namespace 在线阅读
{
     public class Book
    {
         public string Name { get; set; }

         public string Url { get; set; }

         public Book(string name,string url)
         {
             this.Name = name;
             this.Url = url;
         }
    }
}
