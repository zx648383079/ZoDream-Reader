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
        var text = "【科幻】《书名》作者：明";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "书名");
        Assert.AreEqual(a, "明");
        Assert.AreEqual(c, "科幻");
    }
    [TestMethod]
    public void TestName1()
    {
        var text = "《书名》明";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "书名");
        Assert.AreEqual(a, "明");
        Assert.AreEqual(c, string.Empty);
    }
    [TestMethod]
    public void TestName2()
    {
        var text = "[科幻]书名 作者：明";
        var res = TxtReader.Parse(text, out var a, out var c);
        Assert.AreEqual(res, "书名");
        Assert.AreEqual(a, "明");
        Assert.AreEqual(c, "科幻");
    }
}