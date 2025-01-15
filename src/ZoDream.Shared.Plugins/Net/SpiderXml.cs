using AngleSharp;
using AngleSharp.Dom;
using System;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderXml : IQueryableObject
    {

        public SpiderXml(NetSpider spider, string content)
        {
            _spider = spider;
            _doc = BrowsingContext.New(Configuration.Default.WithXml()).OpenAsync(req => req.Content(content)).GetAwaiter().GetResult();
        }

        private readonly NetSpider _spider;
        private readonly IDocument _doc;

        public IQueryableObject Query(string selector)
        {
            throw new NotImplementedException();
        }

        public ITextObject Text()
        {
            return new SpiderText(_spider, _doc.TextContent);
        }

        public IBaseObject Clone()
        {
            return new SpiderXml(_spider, string.Empty);
        }

        public IBaseObject As(string name)
        {
            throw new NotImplementedException();
        }
    }
}
