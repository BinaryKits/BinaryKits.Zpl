using BinaryKits.ZplUtility.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace BinaryKits.ZplUtility.UnitTest
{
    [TestClass]
    public class BarcodeTest
    {
        [TestMethod]
        public void Barcode39()
        {
            var elements = new List<ZplElementBase>
            {
                new ZplBarcode39("123ABC", 100, 100)
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,100\n^B3N,N,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }

        [TestMethod]
        public void Barcode128()
        {
            var elements = new List<ZplElementBase>
            {
                new ZplBarcode128("123ABC", 100, 300)
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,300\n^BN,100,Y,N\n^FD123ABC^FS\n^XZ", output);
        }

        [TestMethod]
        public void BarcodeEan13()
        {
            var elements = new List<ZplElementBase>
            {
                new ZplBarcodeEan13("123456", 100, 300)
            };

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n\n^FO100,300\n^BEN,100,Y,N\n^FD123456^FS\n^XZ", output);
        }
    }
}
