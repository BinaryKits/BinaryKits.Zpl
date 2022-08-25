using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class LabelHomeCommandTest
    {
        [TestMethod]
        public void ToZpl_X10Y10_Successful()
        {
            var command = new LabelHomeCommand(10, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LH10,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_X0Y0_Successful()
        {
            var command = new LabelHomeCommand(0, 0);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LH0,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_XnullY10_Successful()
        {
            var command = new LabelHomeCommand(null, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LH0,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_XnullYnull_Successful()
        {
            var command = new LabelHomeCommand(null, null);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LH0,0", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_XMinus10Y0_Exception()
        {
            new LabelHomeCommand(-10, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_X0YMinus10_Exception()
        {
            new LabelHomeCommand(0, -10);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = LabelHomeCommand.CanParseCommand("^LH10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = LabelHomeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^LH10,10");
            Assert.IsTrue(command is LabelHomeCommand);
            if (command is LabelHomeCommand homeCommand)
            {
                Assert.AreEqual(10, homeCommand.X);
                Assert.AreEqual(10, homeCommand.Y);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^LH0,20");
            Assert.IsTrue(command is LabelHomeCommand);
            if (command is LabelHomeCommand homeCommand)
            {
                Assert.AreEqual(0, homeCommand.X);
                Assert.AreEqual(20, homeCommand.Y);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommandXisEmpty_Successful()
        {
            var command = CommandBase.ParseCommand("^LH,10");
            Assert.IsTrue(command is LabelHomeCommand);
            if (command is LabelHomeCommand homeCommand)
            {
                Assert.AreEqual(0, homeCommand.X);
                Assert.AreEqual(10, homeCommand.Y);
            }
        }

    }
}
