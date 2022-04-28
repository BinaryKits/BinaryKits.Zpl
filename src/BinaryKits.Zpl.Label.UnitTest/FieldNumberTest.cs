using System;
using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class FieldNumberTest
    {
        [TestMethod]
        public void SingleElement()
        {
            var textField = new ZplTextField(null, 50, 100, ZplConstants.Font.Default);
            var output = new ZplFieldNumber(100, textField).ToZplString();

            Assert.IsNotNull(output);
            Assert.AreEqual("^A0N,30,30\n^FO50,100\n^FH\n^FN100^FS", output);
        }

        [TestMethod]
        public void WrongTypeElement()
        {
            var data = new ZplReferenceGrid();
            Assert.ThrowsException<ArgumentException>(() => new ZplFieldNumber(100, data).ToZplString());
        }
    }
}