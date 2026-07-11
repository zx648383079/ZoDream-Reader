using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using ZoDream.Shared.Plugins.Own;
using ZoDream.Shared.Text;

namespace ZoDream.Tests
{
    [TestClass]
    public class PluginTest
    {
        [TestMethod]
        public void TestXml()
        {
            var reader = XDocument.Load(new StringReader("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<container xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\" version=\"1.0\">\r\n  <rootfiles>\r\n    <rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/>\r\n  </rootfiles>\r\n</container>"));
            XNamespace cnsNamespace = "urn:oasis:names:tc:opendocument:xmlns:container";
            var node = reader.Element(cnsNamespace + "container");
            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void TestConvert()
        {
            var baseFolder = "";
            var oldDict = "";
            var newDict = "";
            var maxTime = File.GetLastWriteTime(newDict);
            var now = DateTime.Now;
            var oldEncoding = new OwnEncoding(OwnDictionary.OpenFile(oldDict));
            var newEncoding = new OwnEncoding(OwnDictionary.OpenFile(newDict));
            var i = 0;
            foreach (var item in Directory.GetFiles(baseFolder, "*.npk", SearchOption.AllDirectories))
            {
                Debug.WriteLine("Processing file: " + item);
                var current = File.GetLastWriteTime(item);
                if (current > maxTime)
                {
                    Debug.WriteLine("Skipping file: " + item);
                    continue;
                }
                using var fs = File.Open(item, FileMode.Open, FileAccess.ReadWrite);
                var doc = new OwnReader(fs, oldEncoding).Read();
                if (doc is null)
                {
                    continue;
                }
                fs.Seek(0, SeekOrigin.Begin);
                new OwnWriter(doc, newEncoding).Write(fs);
                fs.Flush();
                fs.SetLength(fs.Position);
                File.SetLastWriteTime(item, now);
                i++;
            }
            Assert.IsTrue(i > 1);
        }
    }
}
