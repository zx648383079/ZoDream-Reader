using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Text;

namespace ZoDream.Tests
{
    [TestClass]
    public class EncodingTest
    {

        //[TestMethod]
        public void TestBuilder()
        {
            var fileName = "dict.bin";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var builder = EncodingBuilder.OpenFile(fileName);
            var items = builder.TraditionalItems.ToArray();
            builder.AppendFile("1.txt");
            builder.AppendFile("2.txt");
            builder.AppendFile("3.txt");
            builder.SaveAs(fileName);
            Assert.AreEqual(builder.Count, 5035);
        }

        //[TestMethod]
        public void TestConvert()
        {
            var fileName = "dict.bin";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var builder = OwnDictionary.OpenFile(fileName);
            var encoding = new OwnEncoding(builder);
            var buffer = new byte[2000];
            var text = "你好";
            var i = encoding.GetBytes(text, buffer);
            Assert.AreEqual(text, encoding.GetString(buffer, 0, i));
            text = "你也好";
            i = encoding.GetBytes(text, buffer);
            Assert.AreEqual(text, encoding.GetString(buffer, 0, i));
        }
    }
}
