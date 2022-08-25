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
            var isParsable = BarCodeFieldDefaultCommand.CanParseCommand("^BY2,2.9,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = BarCodeFieldDefaultCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^BY2,2.9,10");
            Assert.IsTrue(command is BarCodeFieldDefaultCommand);
            if (command is BarCodeFieldDefaultCommand barcodeCommand)
            {
                Assert.AreEqual(2, barcodeCommand.ModuleWidth);
                Assert.AreEqual(2.9, barcodeCommand.WideBarToNarrowBarWidthRatio);
                Assert.AreEqual(10, barcodeCommand.BarCodeHeight);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^BY5,2.0,50");
            Assert.IsTrue(command is BarCodeFieldDefaultCommand);
            if (command is BarCodeFieldDefaultCommand barcodeCommand)
            {
                Assert.AreEqual(5, barcodeCommand.ModuleWidth);
                Assert.AreEqual(2.0, barcodeCommand.WideBarToNarrowBarWidthRatio);
                Assert.AreEqual(50, barcodeCommand.BarCodeHeight);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommandXisEmpty_Successful()
        {
            var command = CommandBase.ParseCommand("^BY2,2.9,100");
            Assert.IsTrue(command is BarCodeFieldDefaultCommand);
            if (command is BarCodeFieldDefaultCommand barcodeCommand)
            {
                Assert.AreEqual(2, barcodeCommand.ModuleWidth);
                Assert.AreEqual(2.9, barcodeCommand.WideBarToNarrowBarWidthRatio);
                Assert.AreEqual(100, barcodeCommand.BarCodeHeight);
            }
        }

    }
}
