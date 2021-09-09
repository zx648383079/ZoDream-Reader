using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZoDream.Tests;
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        var a = 4;
        a -= 1 - 1;
        Assert.AreEqual(2, a);
    }
}