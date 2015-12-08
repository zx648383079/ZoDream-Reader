/*
 * 正则表达式的组成
 * 
 */

namespace 在线阅读.Models
{
    public class BookRegex
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Web { get; set; }
        /// <summary>
        /// 章节名及网址：正则表达式中必须包含"title"、"url>"
        /// </summary>
        public string Chapter { get; set; }
        /// <summary>
        /// 正文：正则表达式中必须包含"content"
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 正文中替换的内容
        /// </summary>
        public string Replace { get; set; }

        /// <summary>
        /// 小说的正则表达式
        /// </summary>
        /// <param name="web">网站域名</param>
        /// <param name="chapter">章节名及网址：正则表达式中必须包含"title"、"url>"</param>
        /// <param name="content">正文：正则表达式中必须包含"content"</param>
        /// <param name="replace">正文中替换的内容</param>
        public BookRegex(string web, string chapter, string content,string replace)
        {
            this.Web = web;
            this.Chapter = chapter;
            this.Content = content;
            this.Replace = replace;
        }
    }
}
