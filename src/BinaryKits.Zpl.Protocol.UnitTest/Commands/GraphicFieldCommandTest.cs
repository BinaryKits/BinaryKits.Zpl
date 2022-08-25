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
            var isParsable = GraphicFieldCommand.CanParseCommand("^GFA,4,4,2,A1B2C3D4");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = GraphicFieldCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^GFA,4,4,2,A1B2C3D4");
            Assert.IsTrue(command is GraphicFieldCommand);
            if (command is GraphicFieldCommand fieldCommand)
            {
                Assert.AreEqual('A', fieldCommand.CompressionType);
                Assert.AreEqual(4, fieldCommand.BinaryByteCount);
                Assert.AreEqual(4, fieldCommand.GraphicFieldCount);
                Assert.AreEqual(2, fieldCommand.BytesPerRow);
                Assert.AreEqual("A1B2C3D4", fieldCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^GFB,4,4,,A1B2C3D4");
            Assert.IsTrue(command is GraphicFieldCommand);
            if (command is GraphicFieldCommand fieldCommand)
            {
                Assert.AreEqual('B', fieldCommand.CompressionType);
                Assert.AreEqual(4, fieldCommand.BinaryByteCount);
                Assert.AreEqual(4, fieldCommand.GraphicFieldCount);
                Assert.AreEqual(0, fieldCommand.BytesPerRow);
                Assert.AreEqual("A1B2C3D4", fieldCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^GFB,,,,A1B2C3D4");
            Assert.IsTrue(command is GraphicFieldCommand);
            if (command is GraphicFieldCommand fieldCommand)
            {
                Assert.AreEqual('B', fieldCommand.CompressionType);
                Assert.AreEqual(0, fieldCommand.BinaryByteCount);
                Assert.AreEqual(0, fieldCommand.GraphicFieldCount);
                Assert.AreEqual(0, fieldCommand.BytesPerRow);
                Assert.AreEqual("A1B2C3D4", fieldCommand.Data);
            }
        }

    }
}
