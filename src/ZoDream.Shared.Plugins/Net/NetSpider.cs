using System;
using System.Collections.Generic;
using System.Net.Http;
using ZoDream.Shared.Http;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Script.Interfaces;

namespace ZoDream.Shared.Plugins.Net
{
    public class NetSpider : ISpider, IGobalFactory
    {
        public NetSpider()
        {
            
        }
        public NetSpider(string uri)
        {
            Uri.TryCreate(uri, UriKind.Absolute, out BaseUri);
        }

        private readonly Uri? BaseUri;

        public IHttpClient Create(string url)
        {
            if (BaseUri is not null && !url.Contains("://"))
            {
                url = new Uri(BaseUri, url).ToString();
            }
            return new Client(url);
        }

        public ITextObject Get(string url)
        {
            return new SpiderText(this, Create(url).ReadAsync().GetAwaiter().GetResult() ?? string.Empty);
        }

        public ITextObject Post(string url, IDictionary<string, object> body)
        {
            var client = Create(url);
            client.Body = Convert(body);
            client.Method = RequestMethod.Post;
            return new SpiderText(this, client.ReadAsync().GetAwaiter().GetResult()?? string.Empty);
        }

        public IUrlObject Url(string url)
        {
            return new SpiderUrl(this, url);
        }

        public void Dispose()
        {
        }

        public HttpContent Convert(IDictionary<string, object> data)
        {
            if (false)
            {
                return new JsonStringContent(data).ToHttpContent();
            }
            var res = new MultipartFormDataContent();
            foreach (var item in data)
            {
                res.Add(new StringContent(item.Value.ToString()), item.Key);
            }
            return res;
        }

        public static IObjectCollection<K> ToObjectCollection<K>(IEnumerable<K> items)
        {
            if (items is IObjectCollection<K> i)
            {
                return i;
            }
            var res = new SpiderObjectCollection<K>();
            foreach (var item in items)
            {
                res.Add(item);
            }
            return res;
        }
    }
}
