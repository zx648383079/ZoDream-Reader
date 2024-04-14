using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Tests;
[TestClass]
public class PageTest
{
    // [TestMethod]
    public async void TestPages()
    {
        var tokenizer = new PageTokenizer();
        //tokenizer.Width = 600;
        //tokenizer.Height = 600;
        tokenizer.Content = new StreamIterator("D:\\Downloads\\hash.txt");
        await tokenizer.Refresh();
        var page = await tokenizer.GetAsync();
        var data = tokenizer.CachePages[0];
        tokenizer.Dispose();
        Assert.IsTrue(page[0].End == data.End);
    }
}