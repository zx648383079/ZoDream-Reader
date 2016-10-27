using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.Helper
{
    public class HttpHelper
    {
        public static List<ChapterItem> GetBook(ref BookItem item, WebRuleItem rule)
        {
            var html = new HtmlExpand();
            html.SetUrl(item.Url);
            if (!string.IsNullOrWhiteSpace(rule.CoverBegin) || !string.IsNullOrWhiteSpace(rule.CoverEnd))
            {
                item.Image = html.GetCover(rule.CoverBegin, rule.CoverEnd);
            }
            if (!string.IsNullOrWhiteSpace(rule.AuthorBegin) || !string.IsNullOrWhiteSpace(rule.AuthorEnd))
            {
                item.Author = html.GetAuthor(rule.AuthorBegin, rule.AuthorEnd);
            }
            if (!string.IsNullOrWhiteSpace(rule.DescriptionBegin) || !string.IsNullOrWhiteSpace(rule.DescriptionEnd))
            {
                item.Description = html.GetDescription(rule.DescriptionBegin, rule.DescriptionEnd);
            }
            var chapters = GetChapters(item, rule, html);
            item.Count = chapters.Count;
            return chapters;
        }

        public static List<ChapterItem> GetChapters(BookItem item, WebRuleItem rule, HtmlExpand html)
        {
            var chapters = new List<ChapterItem>();
            var ms =
                html.Narrow(rule.CatalogBegin, rule.CatalogEnd)
                    .Matches(@"<a[^<>]+?href=""?(?<href>[^""<>\s]+)[^<>]*>(?<title>[\s\S]+?)</a>");
            foreach (Match match in ms)
            {
                var url = match.Groups["href"].Value;
                chapters.Add(new ChapterItem(match.Groups["title"].Value, UrlHelper.GetAbsolute(item.Url, url),
                    LocalHelper.GetSafeFile(url)));
            }
            return chapters;
        }
    }
}
