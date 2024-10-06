using AngleSharp;
using AngleSharp.Dom;
using System;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderHtml : IQueryableObject
    {

        public SpiderHtml(NetSpider spider, string content)
        {
            _spider = spider;
            _doc = BrowsingContext.New(Configuration.Default).OpenAsync(req => req.Content(content)).GetAwaiter().GetResult();
        }

        private readonly NetSpider _spider;
        private readonly IDocument _doc;

        public IQueryableObject Query(string selector)
        {
            _doc.QuerySelectorAll(selector);
        }

        public ITextObject Text()
        {
            return new SpiderText(_spider, _doc.TextContent);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
