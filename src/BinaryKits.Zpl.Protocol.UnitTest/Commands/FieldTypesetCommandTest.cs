using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldTypesetCommandTest
    {
        [TestMethod]
        public void ToZpl_X10Y10_Successful()
        {
            var command = new FieldTypesetCommand(10, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FT10,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_X0Y0_Successful()
        {
            var command = new FieldTypesetCommand(0, 0);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FT0,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_XnullY10_Successful()
        {
            var command = new FieldTypesetCommand(null, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FT0,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_XnullYnull_Successful()
        {
            var command = new FieldTypesetCommand(null, null);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FT0,0", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_XMinus10Y0_Exception()
        {
            new FieldTypesetCommand(-10, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_X0YMinus10_Exception()
        {
            new FieldTypesetCommand(0, -10);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = FieldTypesetCommand.CanParseCommand("^FT10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = FieldTypesetCommand.CanParseCommand("^FO10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FT10,10");
            Assert.IsTrue(command is FieldTypesetCommand);
            if (command is FieldTypesetCommand typesetCommand)
            {
                Assert.AreEqual(10, typesetCommand.X);
                Assert.AreEqual(10, typesetCommand.Y);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FT0,20");
            Assert.IsTrue(command is FieldTypesetCommand);
            if (command is FieldTypesetCommand typesetCommand)
            {
                Assert.AreEqual(0, typesetCommand.X);
                Assert.AreEqual(20, typesetCommand.Y);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommandXisEmpty_Successful()
        {
            var command = CommandBase.ParseCommand("^FT,10");
            Assert.IsTrue(command is FieldTypesetCommand);
            if (command is FieldTypesetCommand typesetCommand)
            {
                Assert.AreEqual(0, typesetCommand.X);
                Assert.AreEqual(10, typesetCommand.Y);
            }
        }

    }
}
