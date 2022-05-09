using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class RecallFieldNumberTest
    {
        [TestMethod]
        public void SingleElement()
        {
            var output = new ZplRecallFieldNumber(100, "value").ToZplString();

            Assert.IsNotNull(output);
            Assert.AreEqual("^FN100\n^FDvalue^FS", output);
        }
    }
}