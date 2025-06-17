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
    public void TestPages()
    {
        var text = "���ƻá������������ߣ���";
        var novel = new BookEntity();
        new TxtReader().Decode(text, novel);

        Assert.AreEqual(novel.Name, "����");
    }
}