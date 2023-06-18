using BinaryKits.Zpl.Label.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class ZebraZ64CompressionHelperTest
    {


        [TestMethod]
        public void Compress_ValidData1_Successful()
        {
            var compressed = ZebraZ64CompressionHelper.Compress("FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF");
            Assert.AreEqual(":Z64:eJz7/x8GGhj+/2cAYUZsLCgNxNhZMAAA4sMzUg==:f3e9", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00006,02,JFGFH0GFJF
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1^FS
            //^XZ
        }
        [TestMethod]
        public void CompressUncompress_Flow_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";

            string compressed = ZebraZ64CompressionHelper.Compress(originalData);
            string uncompressed = Convert.ToHexString(ZebraZ64CompressionHelper.Uncompress(compressed));
            Assert.AreEqual(originalData, uncompressed);
        }
    }
}
