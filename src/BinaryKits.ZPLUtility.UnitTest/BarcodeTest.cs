using BinaryKits.Utility.ZPLUtility;
using BinaryKits.Utility.ZPLUtility.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZPLUtility.UnitTest
{
    [TestClass]
    public class BarcodeTest
    {
        [TestMethod]
        public void BarCode39()
        {
            var elements = new List<ZPLElementBase>
            {
                new ZPLBarCode39("123ABC", 100, 100)
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,100\n^B3N,N,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }

        [TestMethod]
        public void BarCode128()
        {
            var elements = new List<ZPLElementBase>
            {
                new ZPLBarCode128("123ABC", 100, 300)
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,300\n^BN,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }
    }
}
