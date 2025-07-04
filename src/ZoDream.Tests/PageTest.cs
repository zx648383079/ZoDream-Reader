using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Txt;

namespace ZoDream.Tests;
[TestClass]
public class PageTest
{
    [TestMethod]
    public void TestName()
    {
        var text = "���ƻá������������ߣ���";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "����");
        Assert.AreEqual(a, "��");
        Assert.AreEqual(c, "�ƻ�");
    }
    [TestMethod]
    public void TestName1()
    {
        var text = "����������";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "����");
        Assert.AreEqual(a, "��");
        Assert.AreEqual(c, string.Empty);
    }
    [TestMethod]
    public void TestName2()
    {
        var text = "[�ƻ�]���� ���ߣ���";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "����");
        Assert.AreEqual(a, "��");
        Assert.AreEqual(c, "�ƻ�");
    }

    //[TestMethod]
    public void TestXml()
    {
        var fileName = "1.xhtml";
        var doc = XDocument.Load(fileName);
        var res = doc.Root;
        var text = res.Element((XNamespace)"http://www.w3.org/1999/xhtml" + "body")?.Value;
        Assert.IsTrue(res != null);
    }
}