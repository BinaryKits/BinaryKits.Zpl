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
            new DownloadGraphicsCommand("Z:", "TEST", 10, 5, "0000FFFFFFFFFFFFFF00");
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = DownloadGraphicsCommand.CanParseCommand("~DGR:TEST.GRF,10,5,0000FFFFFFFFFFFFFF00");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = DownloadGraphicsCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("~DGR:TEST.GRF,10,5,0000FFFFFFFFFFFFFF00");
            Assert.IsTrue(command is DownloadGraphicsCommand);
            if (command is DownloadGraphicsCommand graphicsCommand)
            {
                Assert.AreEqual("R:", graphicsCommand.StorageDevice);
                Assert.AreEqual("TEST.GRF", graphicsCommand.ImageName);
                Assert.AreEqual(10, graphicsCommand.TotalNumberOfBytesInGraphic);
                Assert.AreEqual(5, graphicsCommand.NumberOfBytesPerRow);
                Assert.AreEqual("0000FFFFFFFFFFFFFF00", graphicsCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("~DGR:,10,5,0000FFFFFFFFFFFFFF00");
            Assert.IsTrue(command is DownloadGraphicsCommand);
            if (command is DownloadGraphicsCommand graphicsCommand)
            {
                Assert.AreEqual("R:", graphicsCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN.GRF", graphicsCommand.ImageName);
                Assert.AreEqual(10, graphicsCommand.TotalNumberOfBytesInGraphic);
                Assert.AreEqual(5, graphicsCommand.NumberOfBytesPerRow);
                Assert.AreEqual("0000FFFFFFFFFFFFFF00", graphicsCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_NoData_Successful()
        {
            var command = CommandBase.ParseCommand("~DGTEST.GRF");
            Assert.IsTrue(command is DownloadGraphicsCommand);
            if (command is DownloadGraphicsCommand graphicsCommand)
            {
                Assert.AreEqual("R:", graphicsCommand.StorageDevice);
                Assert.AreEqual("TEST.GRF", graphicsCommand.ImageName);
                Assert.AreEqual(0, graphicsCommand.TotalNumberOfBytesInGraphic);
                Assert.AreEqual(0, graphicsCommand.NumberOfBytesPerRow);
                Assert.IsNull(graphicsCommand.Data);
            }
        }

    }
}
