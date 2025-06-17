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
        var text = "【科幻】《书名》作者：明";
        var novel = new BookEntity();
        new TxtReader().Decode(text, novel);

        Assert.AreEqual(novel.Name, "书名");
    }
}