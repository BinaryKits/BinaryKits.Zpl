using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class ImageMoveCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new ImageMoveCommand("R:", "TEST.GRF");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^IMR:TEST.GRF", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new ImageMoveCommand("R:", "TEST.PNG");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^IMR:TEST.PNG", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = ImageMoveCommand.CanParseCommand("^IMR:TEST.PNG");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = ImageMoveCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^IMR:TEST.PNG");
            Assert.IsTrue(command is ImageMoveCommand);
            if (command is ImageMoveCommand moveCommand)
            {
                Assert.AreEqual("R:", moveCommand.StorageDevice);
                Assert.AreEqual("TEST.PNG", moveCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^IMIMAGE.GRF");
            Assert.IsTrue(command is ImageMoveCommand);
            if (command is ImageMoveCommand moveCommand)
            {
                Assert.AreEqual("R:", moveCommand.StorageDevice);
                Assert.AreEqual("IMAGE.GRF", moveCommand.ImageName);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^IMR:");
            Assert.IsTrue(command is ImageMoveCommand);
            if (command is ImageMoveCommand moveCommand)
            {
                Assert.AreEqual("R:", moveCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN.GRF", moveCommand.ImageName);
            }
        }

    }
}
