using System;
using System.IO;
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

        public SpiderText(NetSpider spider, string content, string alias)
            : this (spider, content)
        {
            Alias = alias;
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
            var res = _factory.Array(this);
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
                return (ITextObject)_factory.Null(this);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }

        public ITextObject Match(string pattern, string group)
        {
            var match = Regex.Match(_body, pattern);
            if (!match.Success)
            {
                return (ITextObject)_factory.Null(this);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }

        public IArrayObject Match(Regex pattern)
        {
            var matches = pattern.Matches(_body);
            var res = _factory.Array(this);
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
        public ITextObject Match(Regex pattern, int group)
        {
            var match = pattern.Match(_body);
            if (!match.Success)
            {
                return (ITextObject)_factory.Null(this);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }
        public ITextObject Match(Regex pattern, string group)
        {
            var match = pattern.Match(_body);
            if (!match.Success)
            {
                return (ITextObject)_factory.Null(this);
            }
            return new SpiderText(_factory, match.Groups[group].Value);
        }

        public ITextObject Replace(string pattern, string replacement)
        {
            return new SpiderText(_factory, _body.Replace(pattern, replacement));
        }

        public ITextObject Replace(Regex pattern, string replacement)
        {
            return new SpiderText(_factory, pattern.Replace(_body, replacement));
        }

        public ITextObject Append(string text)
        {
            return new SpiderText(_factory, _body + "\r\n" + text);
        }
        public ITextObject Append(IBaseObject text)
        {
            if (text is IQueryableObject q)
            {
                return Append(q.Text());
            }
            if (text is SpiderText o)
            {
                return Append(o._body);
            }
            return Append(text.ToString());
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

        public override string ToString()
        {
            return _body;
        }
    }
}
