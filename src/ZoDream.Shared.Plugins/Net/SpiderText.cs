using System;
using System.Linq;
using System.Text.RegularExpressions;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderText : ITextObject
    {
        public SpiderText(NetSpider spider, string content)
        {
            _body = content;
            _factory = spider;
            Parent = this;
        }
        private readonly string _body;
        private readonly NetSpider _factory;
        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }
        public bool Empty()
        {
            return string.IsNullOrWhiteSpace(_body);
        }

        public bool Eq(string text)
        {
            return text == _body;
        }

        public IQueryableObject Html()
        {
            return new SpiderHtml(_factory, _body);
        }

        public IQueryableObject Json()
        {
            return new SpiderJson(_factory, _body);
        }

        public IUrlObject Url()
        {
            return new SpiderUrl(_factory, _body);
        }

        public IQueryableObject Xml()
        {
            return new SpiderXml(_factory, _body);
        }

        public IArrayObject Match(string pattern)
        {
            var matches = Regex.Matches(_body, pattern);
            var res = new SpiderArray(_factory);
            if (matches is null || matches.Count == 0)
            {
                return res;
            }
            foreach (var item in matches)
            {
                res.Add(new SpiderText(_factory, item.ToString()));
            }
            return res;
        }

        public ITextObject Match(string pattern, int group)
        {
            var match = Regex.Match(_body, pattern);
            if (!match.Success)
            {
                return new SpiderText(_factory, string.Empty);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }

        public ITextObject Match(string pattern, string group)
        {
            var match = Regex.Match(_body, pattern);
            if (!match.Success)
            {
                return new SpiderText(_factory, string.Empty);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }

        public IArrayObject Split(string tag)
        {
            return _factory.ToArray(_body.Split([tag], StringSplitOptions.None)
                .Select(i => new SpiderText(_factory, i)));
        }

        public IArrayObject Split(string tag, int count)
        {
            return _factory.ToArray(_body.Split([tag], count, StringSplitOptions.None).Select(i => new SpiderText(_factory, i)));
        }

        public IBaseObject Clone()
        {
            return new SpiderText(_factory, _body);
        }


        public IBaseObject Is(IBaseObject condition, IBaseObject trueResult)
        {
            return Is(condition.Empty(), trueResult);
        }

        public IBaseObject Is(IBaseObject condition, IBaseObject trueResult, IBaseObject falseResult)
        {
            return Is(condition.Empty(), trueResult, falseResult);
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult)
        {
            return Is(condition, trueResult, this);
        }

        public IBaseObject Is(bool condition, IBaseObject trueResult, IBaseObject falseResult)
        {
            return condition ? trueResult : falseResult;
        }
    }
}
