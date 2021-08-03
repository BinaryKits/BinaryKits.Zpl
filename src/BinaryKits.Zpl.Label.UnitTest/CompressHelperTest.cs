using BinaryKits.Zpl.Label.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class CompressHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid data")]
        public void CompressHex_EmptyData_Exception()
        {
            var compressed = CompressHelper.CompressHex(string.Empty, 10);
        }

        [TestMethod]
        public void CompressHex_ValidData_Exception()
        {
            var compressed = CompressHelper.CompressHex("FFFF\nF00F\nFFFF\n", 2);
            Assert.AreEqual("!GFH0GF!", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00006,02,!GFH0GF!
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1^FS
            //^XZ
        }

        //[TestMethod]
        //public void CompressHex_ValidData1_Exception()
        //{
        //    var compressed = CompressHelper.CompressHex("FFFFF00FFFFF", 2);
        //    Assert.AreEqual("!GFH0GF!", compressed);
        //    //^XA
        //    //~DGR:SAMPLE.GRF,00006,02,!GFH0GF!
        //    //^FO20,20
        //    //^XGR:SAMPLE.GRF,1,1^FS
        //    //^XZ
        //}

        [TestMethod]
        public void CompressHex_ValidData2_Exception()
        {
            var compressed = CompressHelper.CompressHex("FFFFFFFFFFFFFFFFFFFF\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\n8000FFFF0000FFFF0001\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFF0000FFFF0000FFFF\nFFFFFFFFFFFFFFFFFFFF\n", 10);
            Assert.AreEqual("!G8I0JFJ0JFI0G1::JFJ0JFJ0JF::!", compressed);
            //^XA
            //~DGR:SAMPLE.GRF,00080,010,!G8I0JFJ0JFI0G1::JFJ0JFJ0JF::!
            //^FO20,20
            //^XGR:SAMPLE.GRF,1,1 ^ FS
            //^XZ
        }
    }
}
