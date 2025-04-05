using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ZoDream.Shared.Storage;

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

    }
}
