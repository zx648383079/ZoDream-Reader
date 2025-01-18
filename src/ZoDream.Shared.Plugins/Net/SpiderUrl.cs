using System;
using System.Collections.Generic;
using ZoDream.Shared.Http;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderUrl : IUrlObject
    {

        public SpiderUrl(NetSpider spider, string url)
        {
            Parent = this;
            _factory = spider;
            _url = url;
        }
        private readonly NetSpider _factory;
        private string _url;
        private string? _proxy;
        private readonly Dictionary<string, string> _headers = [];

        private readonly Dictionary<string, string> _queries = [];

        private RequestMethod _method = RequestMethod.Get;

        public string Alias { get; private set; } = string.Empty;
        public IBaseObject Parent { get; private set; }
        public IBaseObject As(string name)
        {
            Alias = name;
            return this;
        }

        public IUrlObject Method(string method)
        {
            _method = method.ToUpper() switch
            {
                "POST" => RequestMethod.Post,
                "PUT" => RequestMethod.Put,
                "DELETE" => RequestMethod.Delete,
                "HEAD" => RequestMethod.Head,
                "OPTIONS" => RequestMethod.Options,
                "TRACE" => RequestMethod.Trace,
                _ => RequestMethod.Get,
            };
            return this;
        }

        public IUrlObject Header(string key, string value)
        {
            _headers.Add(key, value);
            return this;
        }

        public IUrlObject Header(IDictionary<string, object> headers)
        {
            foreach (var item in headers)
            {
                _headers.Add(item.Key, item.Value.ToString());
            }
            return this;
        }

        public IUrlObject Proxy(string proxy)
        {
            _proxy = proxy;
            return this;
        }

        public IUrlObject Query(string key, string value)
        {
            _queries.Add(key, value);
            return this;
        }

        public IUrlObject Query(IDictionary<string, object> queries)
        {
            foreach (var item in queries)
            {
                _queries.Add(item.Key, item.Value.ToString());
            }
            return this;
        }

        public ITextObject Execute()
        {
            return Execute(null);
        }

        public ITextObject Execute(Action<IHttpClient>? cb)
        {
            var client = _factory.Create(_url);
            RestRequest.AppendPath(client, string.Empty, _queries);
            foreach (var item in _headers)
            {
                client.Headers.Add(item);
            }
            client.Method = _method;
            cb?.Invoke(client);
            return new SpiderText(_factory, client.ReadAsync().GetAwaiter().GetResult() ?? string.Empty);
        }

        public ITextObject Get()
        {
            _method = RequestMethod.Get;
            return Execute();
        }

        public ITextObject Post(IDictionary<string, object> body)
        {
            _method = RequestMethod.Post;
            return Execute(c => {
                c.Body = _factory.Convert(body);
            });
        }


        public IBaseObject Clone()
        {
            return new SpiderUrl(_factory, _url)
            {

            };
        }

        public bool Empty()
        {
            return false;
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
            return _url;
        }
    }
}
