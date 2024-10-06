using System;
using System.Linq;
using System.Text.RegularExpressions;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderText(NetSpider spider, string content) : ITextObject
    {
        public bool Empty()
        {
            return string.IsNullOrWhiteSpace(content);
        }

        public bool Eq(string text)
        {
            return text == content;
        }

        public IQueryableObject Html()
        {
            return new SpiderHtml(spider, content);
        }

        public IQueryableObject Json()
        {
            return new SpiderJson(spider, content);
        }

        public IUrlObject Url()
        {
            return new SpiderUrl(spider, content);
        }

        public IQueryableObject Xml()
        {
            return new SpiderXml(spider, content);
        }

        public IObjectCollection<ITextObject> Match(string pattern)
        {
            var matches = Regex.Matches(content, pattern);
            var res = new SpiderObjectCollection<ITextObject>();
            if (matches is null || matches.Count == 0)
            {
                return res;
            }
            foreach (var item in matches)
            {
                res.Add(new SpiderText(spider, item.ToString()));
            }
            return res;
        }

        public ITextObject Match(string pattern, int group)
        {
            var match = Regex.Match(content, pattern);
            if (!match.Success)
            {
                return new SpiderText(spider, string.Empty);
            }
            return new SpiderText(spider, match.Groups[group].Value);
        }

        public ITextObject Match(string pattern, string group)
        {
            var match = Regex.Match(content, pattern);
            if (!match.Success)
            {
                return new SpiderText(spider, string.Empty);
            }
            return new SpiderText(spider, match.Groups[group].Value);
        }

        public IObjectCollection<ITextObject> Split(string tag)
        {
            return NetSpider.ToObjectCollection<ITextObject>(content.Split(new string[] { tag }, StringSplitOptions.None).Select(i => new SpiderText(spider, i)));
        }

        public IObjectCollection<ITextObject> Split(string tag, int count)
        {
            return NetSpider.ToObjectCollection<ITextObject>(content.Split(new string[] { tag }, count, StringSplitOptions.None).Select(i => new SpiderText(spider, i)));
        }

        public object Clone()
        {
            return new SpiderText(spider, content);
        }
    }
}
