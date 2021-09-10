using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Tests;
[TestClass]
public class PageTest
{
    [TestMethod]
    public void TestPages()
    {
        var tokenizer = new PageTokenizer();
        tokenizer.Width = 600;
        tokenizer.Height = 600;
        tokenizer.Content = new StreamIterator("D:\\Downloads\\ÃÏΩµ…Ò∆ﬁ.txt");
        tokenizer.Refresh();
        var page = tokenizer.Get();
        var data = tokenizer.CachePages[0];
        tokenizer.Dispose();
        Assert.IsTrue(page[0].End == data.End);
    }
}