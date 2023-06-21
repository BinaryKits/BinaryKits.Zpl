using BinaryKits.Zpl.Label.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class ZebraB64CompressionHelperTest
    {
        [TestMethod]
        public void Compress_ValidData1_Successful()
        {
            var compressed = ZebraB64CompressionHelper.Compress("FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF");
            Assert.AreEqual(":B64://///////////4AA//8AAP//AAGAAP//AAD//wABgAD//wAA//8AAf//AAD//wAA/////wAA//8AAP////8AAP//AAD///////////////8=:e4b3", compressed);
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

            string compressed = ZebraB64CompressionHelper.Compress(originalData);
            string uncompressed = ZebraB64CompressionHelper.Uncompress(compressed).ToHexFromBytes();
            Assert.AreEqual(originalData, uncompressed);
        }
    }
}
