using System;
using System.Collections.Generic;
using System.Net.Http;
using ZoDream.Shared.Http;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class SpiderUrl(NetSpider spider, string url) : IUrlObject
    {
        private string? _proxy;


        private readonly Dictionary<string, string> _headers = [];

        private readonly Dictionary<string, string> _queries = [];

        private RequestMethod _method = RequestMethod.Get;


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
            var client = spider.Create(url);
            RestRequest.AppendPath(client, string.Empty, _queries);
            foreach (var item in _headers)
            {
                client.Headers.Add(item);
            }
            client.Method = _method;
            cb?.Invoke(client);
            return new SpiderText(spider, client.ReadAsync().GetAwaiter().GetResult() ?? string.Empty);
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
                c.Body = spider.Convert(body);
            });
        }
    }
}
