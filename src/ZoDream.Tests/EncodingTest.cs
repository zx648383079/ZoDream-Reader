using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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
        const string BaseFolder = "D:\\zodream";
        const string TxtFolder = BaseFolder + "\\txt";

        [TestMethod]
        public void TestBuilder()
        {
            var fileName = Path.Combine(BaseFolder, "dict.bin");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var builder = new EncodingBuilder();
            foreach (var item in Directory.GetFiles(TxtFolder, "*.txt"))
            {
                builder.Append(Path.GetFileNameWithoutExtension(item));
                builder.AppendFile(item);
            }
            builder.Replace(builder.TraditionalItems.ToArray());
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
