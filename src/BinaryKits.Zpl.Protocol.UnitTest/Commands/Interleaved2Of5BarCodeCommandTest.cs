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
            var command = new Interleaved2Of5BarCodeCommand();
            var isParsable = command.IsCommandParsable("^B2N,,N,N,Y");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2N,,N,N,Y");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsTrue(command.CalculateAndPrintMod10CheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2N,,Y,Y,Y");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsTrue(command.PrintInterpretationLineAboveCode);
            Assert.IsTrue(command.CalculateAndPrintMod10CheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2N,,N,N,N");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.CalculateAndPrintMod10CheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2R,20,N,N,N");
            Assert.AreEqual(Orientation.Rotated90, command.Orientation);
            Assert.AreEqual(20, command.BarCodeHeight);
            Assert.IsFalse(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.CalculateAndPrintMod10CheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2B,55");
            Assert.AreEqual(Orientation.Rotated270, command.Orientation);
            Assert.AreEqual(55, command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.CalculateAndPrintMod10CheckDigit);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = new Interleaved2Of5BarCodeCommand();
            command.ParseCommand("^B2");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.IsNull(command.BarCodeHeight);
            Assert.IsTrue(command.PrintInterpretationLine);
            Assert.IsFalse(command.PrintInterpretationLineAboveCode);
            Assert.IsFalse(command.CalculateAndPrintMod10CheckDigit);
        }
    }
}
