using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class DownloadFormatCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new DownloadFormatCommand("R:", "TEST.ZPL");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^DFR:TEST.ZPL", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new DownloadFormatCommand("R:", "TEST.ZPL");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^DFR:TEST.ZPL", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = DownloadFormatCommand.CanParseCommand("^DFR:TEST.ZPL");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = DownloadFormatCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^DFR:TEST.ZPL");
            Assert.IsTrue(command is DownloadFormatCommand);
            if (command is DownloadFormatCommand downloadCommand)
            {
                Assert.AreEqual("R:", downloadCommand.StorageDevice);
                Assert.AreEqual("TEST.ZPL", downloadCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^DFSAMPLE.ZPL");
            Assert.IsTrue(command is DownloadFormatCommand);
            if (command is DownloadFormatCommand downloadCommand)
            {
                Assert.AreEqual("R:", downloadCommand.StorageDevice);
                Assert.AreEqual("SAMPLE.ZPL", downloadCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^DFR:");
            Assert.IsTrue(command is DownloadFormatCommand);
            if (command is DownloadFormatCommand downloadCommand)
            {
                Assert.AreEqual("R:", downloadCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN.ZPL", downloadCommand.ImageName);
            }
        }

    }
}
