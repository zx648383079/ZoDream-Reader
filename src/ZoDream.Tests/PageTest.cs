using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Tokenizers;

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
}