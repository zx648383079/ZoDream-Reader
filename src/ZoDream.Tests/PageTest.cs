using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
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
        var novel = new BookEntity();
        new TxtReader().Decode("【科幻】《书名》作者：明", novel);

        Assert.IsTrue(string.IsNullOrWhiteSpace(novel.Name));
    }
}