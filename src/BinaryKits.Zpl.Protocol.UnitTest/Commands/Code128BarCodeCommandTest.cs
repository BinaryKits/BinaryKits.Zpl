using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class Code128BarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new Code128BarCodeCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BCN,,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new Code128BarCodeCommand(Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BCI,,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new Code128BarCodeCommand(barCodeHeight: 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BCN,10,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default4_Successful()
        {
            var command = new Code128BarCodeCommand(printInterpretationLine: false, printInterpretationLineAboveCode: false, uccCheckDigit: true);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BCN,,N,N,Y", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight1_Exception()
        {
            new Code128BarCodeCommand(barCodeHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight2_Exception()
        {
            new Code128BarCodeCommand(barCodeHeight: 33000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new Code128BarCodeCommand();
            var isParsable = command.IsCommandParsable("^BCN,,N,N,Y");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new Code128BarCodeCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BCN,,N,N,Y");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsTrue(command.UccCheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BCN,,Y,Y,Y");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsTrue(command.PrintInterpretationLineAboveCode);
            Assert.IsTrue(command.UccCheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BCN,,N,N,N");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.UccCheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BCR,20,N,N,N");
            Assert.AreEqual(Orientation.Rotated90, command.Orientation);
            Assert.AreEqual(20, command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.UccCheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BCB,55");
            Assert.AreEqual(Orientation.Rotated270, command.Orientation);
            Assert.AreEqual(55, command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.UccCheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = new Code128BarCodeCommand();
            command.ParseCommand("^BC");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.UccCheckDigit);
        }
    }
}
