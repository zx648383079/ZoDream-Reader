/*
 * 正则表达式的组成
 * 
 */

namespace 在线阅读
{
    public class BookRegex
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Web { get; set; }
        /// <summary>
        /// 书名：正则表达式中必须包含"name"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 章节名及网址：正则表达式中必须包含"title"、"url>"
        /// </summary>
        public string Chapter { get; set; }
        /// <summary>
        /// 正文：正则表达式中必须包含"content"
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 小说的正则表达式
        /// </summary>
        /// <param name="web">网站域名</param>
        /// <param name="name">书名：正则表达式中必须包含"name"</param>
        /// <param name="chapter">章节名及网址：正则表达式中必须包含"title"、"url>"</param>
        /// <param name="content">正文：正则表达式中必须包含"content"</param>
        public BookRegex(string web, string name, string chapter, string content)
        {
            this.Web = web;
            this.Name = name;
            this.Chapter = chapter;
            this.Content = content;
        }
    }
}
