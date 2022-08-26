using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class RecallFormatCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new RecallFormatCommand("R:", "TEST.ZPL");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XFR:TEST.ZPL", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new RecallFormatCommand("R:", "TEST.ZPL");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XFR:TEST.ZPL", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = RecallFormatCommand.CanParseCommand("^XFR:TEST.ZPL");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = RecallFormatCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^XFR:TEST.ZPL");
            Assert.IsTrue(command is RecallFormatCommand);
            if (command is RecallFormatCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("TEST.ZPL", recallCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^XFSAMPLE.ZPL");
            Assert.IsTrue(command is RecallFormatCommand);
            if (command is RecallFormatCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("SAMPLE.ZPL", recallCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^XFR:");
            Assert.IsTrue(command is RecallFormatCommand);
            if (command is RecallFormatCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN.ZPL", recallCommand.ImageName);
            }
        }

    }
}
