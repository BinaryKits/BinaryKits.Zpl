using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class Interleaved2Of5BarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B2N,,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand(Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B2I,,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand(barCodeHeight: 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B2N,10,Y,N,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default4_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand(printInterpretationLine: false, printInterpretationLineAboveCode: false, calculateAndPrintMod10CheckDigit: true);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B2N,,N,N,Y", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight1_Exception()
        {
            new Interleaved2Of5BarCodeCommand(barCodeHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight2_Exception()
        {
            new Interleaved2Of5BarCodeCommand(barCodeHeight: 33000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = Interleaved2Of5BarCodeCommand.CanParseCommand("^B2N,,N,N,Y");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = Interleaved2Of5BarCodeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^B2N,,N,N,Y");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.IsNull(barCodeCommand.BarCodeHeight);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLine);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsTrue(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^B2N,,Y,Y,Y");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.IsNull(barCodeCommand.BarCodeHeight);
                Assert.IsTrue(barCodeCommand.PrintInterpretationLine);
                Assert.IsTrue(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsTrue(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^B2N,,N,N,N");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.IsNull(barCodeCommand.BarCodeHeight);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLine);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsFalse(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^B2R,20,N,N,N");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Rotated90, barCodeCommand.Orientation);
                Assert.AreEqual(20, barCodeCommand.BarCodeHeight);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLine);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsFalse(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = CommandBase.ParseCommand("^B2B,55");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Rotated270, barCodeCommand.Orientation);
                Assert.AreEqual(55, barCodeCommand.BarCodeHeight);
                Assert.IsTrue(barCodeCommand.PrintInterpretationLine);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsFalse(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = CommandBase.ParseCommand("^B2");
            Assert.IsTrue(command is Interleaved2Of5BarCodeCommand);
            if (command is Interleaved2Of5BarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.IsNull(barCodeCommand.BarCodeHeight);
                Assert.IsTrue(barCodeCommand.PrintInterpretationLine);
                Assert.IsFalse(barCodeCommand.PrintInterpretationLineAboveCode);
                Assert.IsFalse(barCodeCommand.CalculateAndPrintMod10CheckDigit);
            }
        }

    }
}
