using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class GraphicFieldCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new GraphicFieldCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GFA,0,0,0,", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new GraphicFieldCommand('A', 1, 1, 1, "AB");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GFA,1,1,1,AB", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new GraphicFieldCommand('A', 4, 4, 2, "A1B2C3D4");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GFA,4,4,2,A1B2C3D4", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_BinaryByteCountTooLow_Exception()
        {
            new GraphicFieldCommand('A', 0, 1, 1, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_BinaryByteCountTooHigh_Exception()
        {
            new GraphicFieldCommand('A', 100000, 1, 1, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_GraphicFieldCountTooLow_Exception()
        {
            new GraphicFieldCommand('A', 1, 0, 1, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_GraphicFieldCountTooHigh_Exception()
        {
            new GraphicFieldCommand('A', 1, 100000, 1, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_BytesPerRowTooLow_Exception()
        {
            new GraphicFieldCommand('A', 1, 1, 0, "AB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_BytesPerRowTooHigh_Exception()
        {
            new GraphicFieldCommand('A', 1, 1, 100000, "AB");
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new GraphicFieldCommand();
            var isParsable = command.IsCommandParsable("^GFA,4,4,2,A1B2C3D4");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new GraphicFieldCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new GraphicFieldCommand();
            command.ParseCommand("^GFA,4,4,2,A1B2C3D4");
            Assert.AreEqual('A', command.CompressionType);
            Assert.AreEqual(4, command.BinaryByteCount);
            Assert.AreEqual(4, command.GraphicFieldCount);
            Assert.AreEqual(2, command.BytesPerRow);
            Assert.AreEqual("A1B2C3D4", command.Data);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new GraphicFieldCommand();
            command.ParseCommand("^GFB,4,4,,A1B2C3D4");
            Assert.AreEqual('B', command.CompressionType);
            Assert.AreEqual(4, command.BinaryByteCount);
            Assert.AreEqual(4, command.GraphicFieldCount);
            Assert.AreEqual(0, command.BytesPerRow);
            Assert.AreEqual("A1B2C3D4", command.Data);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new GraphicFieldCommand();
            command.ParseCommand("^GFB,,,,A1B2C3D4");
            Assert.AreEqual('B', command.CompressionType);
            Assert.AreEqual(0, command.BinaryByteCount);
            Assert.AreEqual(0, command.GraphicFieldCount);
            Assert.AreEqual(0, command.BytesPerRow);
            Assert.AreEqual("A1B2C3D4", command.Data);
        }
    }
}
