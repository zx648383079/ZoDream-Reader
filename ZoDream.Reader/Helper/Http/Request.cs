using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace ZoDream.Reader.Helper.Http
{
    internal class Request
    {
        public string Url { get; set; }

        public string Accept { get; set; } = Accepts.Html;

        public string UserAgent { get; set; } = UserAgents.Firefox;

        public string Cookie { get; set; }

        public Request()
        {
            
        }

        public Request(string url)
        {
            Url = url;
        }

        public string Get(string url)
        {
            Url = url;
            return Get();
        }

        public string Get()
        {
            var request = GetRequest();
            request.Method = "GET";
            _setProperty(request);
            return GetHtml(request);
        }

        public string Post(string url, IDictionary<string, string> param)
        {
            Url = url;
            return Post(param);
        }

        public string Post(IDictionary<string, string> param)
        {
            var request = GetRequest();
            _post(request, param);
            _setProperty(request);
            return GetHtml(request);
        }

        public WebRequest GetRequest()
        {
            WebRequest request = WebRequest.Create(Url);
            request.Credentials = CredentialCache.DefaultCredentials;
            
            return request;
        }

        private void _setProperty(WebRequest request)
        {
            request.Timeout = 600000;
            var headers = new WebHeaderCollection
            {
                {HttpRequestHeader.AcceptEncoding, "gzip, deflate"},
                {HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3"},
                {HttpRequestHeader.CacheControl, "max-age=0"}
            };
            request.Headers = headers;
            _setHeader((HttpWebRequest) request);
            if (!Regex.IsMatch(Url, "^https://")) return;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
        }

        public WebRequest GetRequest(string url)
        {
            Url = url;
            return GetRequest();
        }

        /// <summary>
        /// ssl/https请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="request">HttpWebRequest对象</param>
        private void _setHeader(HttpWebRequest request)
        {
            request.Accept = Accept;
            request.KeepAlive = true;
            request.UserAgent = UserAgent;
            request.Referer = "";
            request.AllowAutoRedirect = true;
            request.ProtocolVersion = HttpVersion.Version11;
            _setCookie(request);
        }

        /// <summary>
        /// 设置请求Cookie
        /// </summary>
        /// <param name="request">HttpWebRequest对象</param>
        private void _setCookie(HttpWebRequest request)
        {
            // 必须实例化，否则响应中获取不到Cookie
            request.CookieContainer = new CookieContainer();
            if (!string.IsNullOrEmpty(Cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = Cookie;
            }
            //if (cookies != null && cookies.CookieCollection != null && cookies.CookieCollection.Count > 0)
            //{
            //    request.CookieContainer.Add(cookies.CookieCollection);
            //}
        }

        private void _post(WebRequest request, IDictionary<string, string> param)
        {
            var data = new StringBuilder(string.Empty);
            foreach (var keyValuePair in param)
            {
                data.AppendFormat("{0}={1}&", keyValuePair.Key, keyValuePair.Value);
            }
            _post(request, data.Remove(data.Length - 1, 1).ToString());
        }


        private void _post(WebRequest request, string args)
        {
            _post(request, Encoding.UTF8.GetBytes(args));
        }

        private void _post(WebRequest request, byte[] args)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = args.Length;
            var stream = request.GetRequestStream();
            stream.Write(args, 0, args.Length);
            stream.Close();
        }

        /// <summary>
        /// 返回响应报文
        /// </summary>
        /// <param name="request">WebRequest对象</param>
        /// <returns>响应对象</returns>
        public string GetHtml(WebRequest request)
        {
            string html;
            using (var response = request.GetResponse())
            {
                html = GetHtml(response);
            }
            return html;
        }

        public string GetHtml(WebResponse response)
        {
            var html = string.Empty;
            #region 判断解压

            if (((HttpWebResponse) response).StatusCode != HttpStatusCode.OK) return html;
            Stream stream = null;
            stream = ((HttpWebResponse)response).ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase) ? new GZipStream(response.GetResponseStream(), mode: CompressionMode.Decompress) : response.GetResponseStream();
            #region 把网络流转成内存流
            var ms = new MemoryStream();
            var buffer = new byte[1024];

            while (true)
            {
                if (stream == null) continue;
                var sz = stream.Read(buffer, 0, 1024);
                if (sz == 0) break;
                ms.Write(buffer, 0, sz);
            }
            #endregion

            var bytes = ms.ToArray();
            html = GetEncoding(bytes, ((HttpWebResponse)response).CharacterSet).GetString(bytes);
            stream.Close();

            #endregion
            return html;
        }

        /// <summary>
        /// 获取HTML网页的编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        public Encoding GetEncoding(byte[] bytes, string charSet)
        {
            var html = Encoding.Default.GetString(bytes);
            var regCharset = new Regex(@"charset\b\s*=\s*""*(?<charset>[^""]*)");
            if (regCharset.IsMatch(html))
            {
                return Encoding.GetEncoding(regCharset.Match(html).Groups["charset"].Value);
            }

            return charSet != string.Empty ? Encoding.GetEncoding(charSet) : Encoding.Default;
        }
    }
}
