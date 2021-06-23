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
        public void Barcode39()
        {
            var elements = new List<ZPLElementBase>
            {
                new ZPLBarcode39("123ABC", 100, 100)
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,100\n^B3N,N,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }

        [TestMethod]
        public void Barcode128()
        {
            var elements = new List<ZPLElementBase>
            {
                new ZPLBarcode128("123ABC", 100, 300)
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,300\n^BN,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }

        [TestMethod]
        public void BarcodeEan13()
        {
            var elements = new List<ZPLElementBase>
            {
                new ZPLBarcodeEan13("123456", 100, 300)
            };

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,300\n^BEN,100,Y,N\n^FD123456^FS\n^XZ", output);
        }
    }
}
