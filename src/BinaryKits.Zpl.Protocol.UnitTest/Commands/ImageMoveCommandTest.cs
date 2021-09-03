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
            var command = new ImageMoveCommand();
            var isParsable = command.IsCommandParsable("^IMR:TEST.PNG");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new ImageMoveCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new ImageMoveCommand();
            command.ParseCommand("^IMR:TEST.PNG");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("TEST.PNG", command.ImageName);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new ImageMoveCommand();
            command.ParseCommand("^IMR:IMAGE.GRF");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("IMAGE.GRF", command.ImageName);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new ImageMoveCommand();
            command.ParseCommand("^IMR:");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("UNKNOWN.GRF", command.ImageName);
        }
    }
}
