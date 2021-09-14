using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class DownloadGraphicsCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new DownloadGraphicsCommand("R:", "TEST.GRF", 10, 5, "0000FFFFFFFFFFFFFF00");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("~DGR:TEST.GRF,10,5,0000FFFFFFFFFFFFFF00", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new DownloadGraphicsCommand("R:", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("~DGR:TEST,10,5,0000FFFFFFFFFFFFFF00", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new DownloadGraphicsCommand("R:", "TEST", 0, 0, "0000FFFFFFFFFFFFFF00");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("~DGR:TEST,0,0,0000FFFFFFFFFFFFFF00", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidDeviceToStoreImage1_Exception()
        {
            new DownloadGraphicsCommand("R", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidDeviceToStoreImage2_Exception()
        {
            new DownloadGraphicsCommand("R2", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidDeviceToStoreImage3_Exception()
        {
            new DownloadGraphicsCommand("R2", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidDeviceToStoreImage4_Exception()
        {
            new DownloadGraphicsCommand("Z:", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new DownloadGraphicsCommand();
            var isParsable = command.IsCommandParsable("~DGR:TEST.GRF,10,5,0000FFFFFFFFFFFFFF00");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new DownloadGraphicsCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new DownloadGraphicsCommand();
            command.ParseCommand("~DGR:TEST.GRF,10,5,0000FFFFFFFFFFFFFF00");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("TEST.GRF", command.ImageName);
            Assert.AreEqual(10, command.TotalNumberOfBytesInGraphic);
            Assert.AreEqual(5, command.NumberOfBytesPerRow);
            Assert.AreEqual("0000FFFFFFFFFFFFFF00", command.Data);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new DownloadGraphicsCommand();
            command.ParseCommand("~DGR:,10,5,0000FFFFFFFFFFFFFF00");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("UNKNOWN.GRF", command.ImageName);
            Assert.AreEqual(10, command.TotalNumberOfBytesInGraphic);
            Assert.AreEqual(5, command.NumberOfBytesPerRow);
            Assert.AreEqual("0000FFFFFFFFFFFFFF00", command.Data);
        }

        [TestMethod]
        public void ParseCommand_NoData_Successful()
        {
            var command = new DownloadGraphicsCommand();
            command.ParseCommand("~DGR:TEST.GRF");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("TEST.GRF", command.ImageName);
            Assert.AreEqual(0, command.TotalNumberOfBytesInGraphic);
            Assert.AreEqual(0, command.NumberOfBytesPerRow);
            Assert.IsNull(command.Data);
        }
    }
}
