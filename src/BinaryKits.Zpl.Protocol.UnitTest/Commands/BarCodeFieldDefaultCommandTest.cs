using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class BarCodeFieldDefaultCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new BarCodeFieldDefaultCommand(2, 3.0, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BY2,3.0,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new BarCodeFieldDefaultCommand(2, 2.91, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BY2,2.9,10", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidModuleWidth1_Exception()
        {
            new BarCodeFieldDefaultCommand(0, 3, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidModuleWidth2_Exception()
        {
            new BarCodeFieldDefaultCommand(11, 3, 10);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new BarCodeFieldDefaultCommand();
            var isParsable = command.IsCommandParsable("^BY2,2.9,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new BarCodeFieldDefaultCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new BarCodeFieldDefaultCommand();
            command.ParseCommand("^BY2,2.9,10");
            Assert.AreEqual(2, command.ModuleWidth);
            Assert.AreEqual(2.9, command.WideBarToNarrowBarWidthRatio);
            Assert.AreEqual(10, command.BarCodeHeight);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new BarCodeFieldDefaultCommand();
            command.ParseCommand("^BY5,2.0,50");
            Assert.AreEqual(5, command.ModuleWidth);
            Assert.AreEqual(2.0, command.WideBarToNarrowBarWidthRatio);
            Assert.AreEqual(50, command.BarCodeHeight);
        }

        [TestMethod]
        public void ParseCommand_ValidCommandXisEmpty_Successful()
        {
            var command = new BarCodeFieldDefaultCommand();
            command.ParseCommand("^BY2,2.9,100");
            Assert.AreEqual(2, command.ModuleWidth);
            Assert.AreEqual(2.9, command.WideBarToNarrowBarWidthRatio);
            Assert.AreEqual(100, command.BarCodeHeight);
        }
    }
}
