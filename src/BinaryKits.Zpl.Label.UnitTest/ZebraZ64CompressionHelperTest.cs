using BinaryKits.Zpl.Label.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO.Compression;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class ZebraZ64CompressionHelperTest
    {
        [TestMethod]
        public void CompressUncompress_Flow_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";

            string compressed = ZebraZ64CompressionHelper.Compress(originalData);
            string uncompressed = ZebraZ64CompressionHelper.Uncompress(compressed).ToHexFromBytes();
            Assert.AreEqual(originalData, uncompressed);
        }
#if NET5_0_OR_GREATER
        [TestMethod]
        public void Compress_ValidData1_Successful()
        {
            var compressed = ZebraZ64CompressionHelper.Compress("FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF");
            Assert.AreEqual(":Z64:eNr7/x8GGhj+/2cAYUZsLCjNAFKJjQUDAOLDM1I=:8f39", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00006,02,JFGFH0GFJF
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1^FS
            //^XZ
        }
        [TestMethod]
        public void DeflateNetStandardSameAsNetCore_Optimal_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";

            var compressionLevel = CompressionLevel.Optimal;
            string compressed = ZebraZ64CompressionHelper.Deflate(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();
            string compressedCore = ZebraZ64CompressionHelper.DeflateCore(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();

            Assert.AreEqual(compressed, compressedCore);
        }
        [TestMethod]
        public void DeflateNetStandardSameAsNetCore_Fastest_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";

            var compressionLevel = CompressionLevel.Fastest;
            string compressed = ZebraZ64CompressionHelper.Deflate(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();
            string compressedCore = ZebraZ64CompressionHelper.DeflateCore(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();

            Assert.AreEqual(compressed, compressedCore);
        }
        [TestMethod]
        public void DeflateNetStandardSameAsNetCore_SmallestSize_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";

            var compressionLevel = CompressionLevel.SmallestSize;
            string compressed = ZebraZ64CompressionHelper.Deflate(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();
            string compressedCore = ZebraZ64CompressionHelper.DeflateCore(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();

            Assert.AreEqual(compressed, compressedCore);
        }
        [TestMethod]
        public void DeflateNetStandardSameAsNetCore_NoCompression_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF8000FFFF0000FFFF00018000FFFF0000FFFF00018000FFFF0000FFFF0001FFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFF0000FFFF0000FFFFFFFFFFFFFFFFFFFFFFFF";
            
            var compressionLevel = CompressionLevel.NoCompression;
            string compressed = ZebraZ64CompressionHelper.Deflate(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();
            string compressedCore = ZebraZ64CompressionHelper.DeflateCore(originalData.ToBytesFromHex(), compressionLevel).ToHexFromBytes();
            
            Assert.AreEqual(compressed, compressedCore);
        }
        [TestMethod]
        public void InflateNetStandardSameAsNetCore_Successful()
        {
            var originalData = "eJz7/x8GGhj+/2cAYUZsLCgNxNhZMAAA4sMzUg==";

            string compressed = ZebraZ64CompressionHelper.Inflate(originalData.FromBase64()).ToHexFromBytes();
            string compressedCore = ZebraZ64CompressionHelper.InflateCore(originalData.FromBase64()).ToHexFromBytes();
            
            Assert.AreEqual(compressed, compressedCore);
        }
#endif
    }
}
