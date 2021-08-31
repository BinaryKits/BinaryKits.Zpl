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
            var command = new LabelHomeCommand();
            var isParsable = command.IsCommandParsable("^LH10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new LabelHomeCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new LabelHomeCommand();
            command.ParseCommand("^LH10,10");
            Assert.AreEqual(10, command.X);
            Assert.AreEqual(10, command.Y);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new LabelHomeCommand();
            command.ParseCommand("^LH0,20");
            Assert.AreEqual(0, command.X);
            Assert.AreEqual(20, command.Y);
        }

        [TestMethod]
        public void ParseCommand_ValidCommandXisEmpty_Successful()
        {
            var command = new LabelHomeCommand();
            command.ParseCommand("^LH,10");
            Assert.AreEqual(0, command.X);
            Assert.AreEqual(10, command.Y);
        }
    }
}
