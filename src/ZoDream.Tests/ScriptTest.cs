using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Plugins.Net;
using ZoDream.Shared.Script;

namespace ZoDream.Tests
{
    [TestClass]
    public class ScriptTest
    {
        [TestMethod]
        public void TestScript()
        {
            var interpreter = new Interpreter();
            var code = "get('http://zodream.cn').html.query('a.href').text";
            //code = "get('http://zodream.cn').html.query('a').map(url:.href,name:.text)";
            //code = "post('http://zodream.cn',{a:'b'}).json.query('b').text.split('-').first";
            //code = "post('http://zodream.cn',{a:'b'}).json.query('b').text.split('-').last";
            //code = "post('http://zodream.cn',{a:'b'}).json.query('b').text.split('-',3).nth(1)";
            //code = "post('http://zodream.cn',{a:'b'}).json.query('b').text.match('use (\\d+)',1)";
            //code = "url('http://zodream.cn').query({a:'b'}).proxy('http://192.168.0.1:8080').header('Referrer','https://www.baid.com').execute.json.query('b').text";
            //code = "get('http://zodream.cn').jsonp.query('b').text";
            //code = "get('http://zodream.cn').html.query('a').if(.text.eq('next'), .url.html.text, 'empty')";
            var res = interpreter.Execute(code, new NetSpider("https://zodream.cn"));
            Assert.IsTrue(res is null);
        }

        [TestMethod]
        public void TestLexer()
        {
            var code = "Main: url('http://zodream.cn').query({a:'b'}).use(true).Is(/\\d/, .value, null).Is('', name: ..text).end";
            var reader = new Lexer(new StringReader(code));
            var items = new List<Token>();
            var token = reader.NextToken();
            items.Add(token);
            // Assert.AreEqual(token.Value, "get");
            while (true)
            {
                token = reader.NextToken();
                items.Add(token);
                if (token.Type == TokenType.Eof)
                {
                    break;
                }
            }
            Assert.IsTrue(items.Count == 43);
        }
    }
}
