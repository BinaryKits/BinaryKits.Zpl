using BinaryKits.Zpl.Label.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class ZebraHexCompressionHelperTest
    {
        [TestMethod]
        [DataRow(1, "G")]
        [DataRow(2, "H")]
        [DataRow(3, "I")]
        [DataRow(4, "J")]
        [DataRow(5, "K")]
        [DataRow(6, "L")]
        [DataRow(7, "M")]
        [DataRow(8, "N")]
        [DataRow(9, "O")]
        [DataRow(10, "P")]
        [DataRow(11, "Q")]
        [DataRow(12, "R")]
        [DataRow(13, "S")]
        [DataRow(14, "T")]
        [DataRow(15, "U")]
        [DataRow(16, "V")]
        [DataRow(17, "W")]
        [DataRow(18, "X")]
        [DataRow(19, "Y")]
        [DataRow(20, "g")]
        [DataRow(40, "h")]
        [DataRow(60, "i")]
        [DataRow(80, "j")]
        [DataRow(100, "k")]
        [DataRow(120, "l")]
        [DataRow(140, "m")]
        [DataRow(160, "n")]
        [DataRow(180, "o")]
        [DataRow(200, "p")]
        [DataRow(220, "q")]
        [DataRow(240, "r")]
        [DataRow(260, "s")]
        [DataRow(280, "t")]
        [DataRow(300, "u")]
        [DataRow(320, "v")]
        [DataRow(340, "w")]
        [DataRow(360, "x")]
        [DataRow(380, "y")]
        [DataRow(400, "z")]
        public void GetZebraCharCount_Simple_Successful(int charRepeatCount, string compressed)
        {
            var zebraCharCount = ZebraHexCompressionHelper.GetZebraCharCount(charRepeatCount);
            Assert.AreEqual(compressed, zebraCharCount);
        }

        [TestMethod]
        [DataRow(21, "gG")]
        [DataRow(22, "gH")]
        [DataRow(23, "gI")]
        [DataRow(24, "gJ")]
        [DataRow(25, "gK")]
        [DataRow(30, "gP")]
        [DataRow(35, "gU")]
        [DataRow(36, "gV")]
        [DataRow(37, "gW")]
        [DataRow(38, "gX")]
        [DataRow(39, "gY")]
        [DataRow(50, "hP")]
        [DataRow(642, "zrH")]
        [DataRow(800, "zz")]
        [DataRow(842, "zzhH")]
        public void GetZebraCharCount_Complex_Successful(int charRepeatCount, string compressed)
        {
            var zebraCharCount = ZebraHexCompressionHelper.GetZebraCharCount(charRepeatCount);
            Assert.AreEqual(compressed, zebraCharCount);
        }

        [TestMethod]
        public void Compress_ValidData1_Successful()
        {
            var compressed = ZebraHexCompressionHelper.Compress("FFFF\nF00F\nFFFF\n", 2);
            Assert.AreEqual("JFGFH0GFJF", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00006,02,JFGFH0GFJF
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1^FS
            //^XZ
        }

        [TestMethod]
        public void Compress_ValidData2_Successful()
        {
            var compressed = ZebraHexCompressionHelper.Compress("FFFFF00FFFFF", 2);
            Assert.AreEqual("JFGFH0GFJF", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00006,02,JFGFH0GFJF
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1^FS
            //^XZ
        }

        [TestMethod]
        public void Compress_ValidData3_Successful()
        {
            var compressed = ZebraHexCompressionHelper.Compress("FFFFFFFFFFFFFFFFFFFF\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFFFFFFFFFFFFFFFFFF\n", 10);
            Assert.AreEqual("gFG8I0JFJ0JFI0!::JFJ0JFJ0JF::gF", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00080,010,gFG8I0JFJ0JFI0!::JFJ0JFJ0JF::gF
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1 ^ FS
            //^XZ
        }

        [TestMethod]
        public void CompressUncompress_Flow_Successful()
        {
            var originalData = "FFFFFFFFFFFFFFFFFFFF\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFFFFFFFFFFFFFFFFFF\n";

            var compressed = ZebraHexCompressionHelper.Compress(originalData, 10);
            var uncompressed = ZebraHexCompressionHelper.Uncompress(compressed, 10);
            Assert.AreEqual(originalData, uncompressed);
        }

        [TestMethod]
        public void Uncompress_ValidData1_Successful()
        {
            var compressedData = "gO0E\n";

            var uncompressed = ZebraHexCompressionHelper.Uncompress(compressedData, 10);
            Assert.AreEqual("00000000000000000000000000000E\n", uncompressed);
        }

        [TestMethod]
        public void Uncompress_ValidData2_Successful()
        {
            var compressedData = "gO0GE\n";

            var uncompressed = ZebraHexCompressionHelper.Uncompress(compressedData, 10);
            Assert.AreEqual("00000000000000000000000000000E\n", uncompressed);
        }
    }
}
