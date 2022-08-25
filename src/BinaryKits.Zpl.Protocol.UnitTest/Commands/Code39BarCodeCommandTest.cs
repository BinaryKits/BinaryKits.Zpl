using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class Code39BarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new Code39BarCodeCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B3N,N,,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new Code39BarCodeCommand(Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B3I,N,,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new Code39BarCodeCommand(barCodeHeight: 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B3N,N,10,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default4_Successful()
        {
            var command = new Code39BarCodeCommand(printInterpretationLine: false, printInterpretationLineAboveCode: false, mod43CheckDigit: true);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^B3N,Y,,N,N", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight1_Exception()
        {
            new Code39BarCodeCommand(barCodeHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight2_Exception()
        {
            new Code39BarCodeCommand(barCodeHeight: 33000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = Code39BarCodeCommand.CanParseCommand("^B3N,N,,Y,N");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = Code39BarCodeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^B3N,N,,Y,N");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Normal, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.IsNull(code39Command.BarCodeHeight);
                Assert.IsTrue(code39Command.PrintInterpretationLine);
                Assert.IsFalse(code39Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^B3N,N,,Y,Y");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Normal, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.IsNull(code39Command.BarCodeHeight);
                Assert.IsTrue(code39Command.PrintInterpretationLine);
                Assert.IsTrue(code39Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^B3N,N,,N,N");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Normal, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.IsNull(code39Command.BarCodeHeight);
                Assert.IsFalse(code39Command.PrintInterpretationLine);
                Assert.IsFalse(code39Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^B3R,N,20,N,N");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Rotated90, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.AreEqual(20, code39Command.BarCodeHeight);
                Assert.IsFalse(code39Command.PrintInterpretationLine);
                Assert.IsFalse(code39Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = CommandBase.ParseCommand("^B3B,,55");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Rotated270, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.AreEqual(55, code39Command.BarCodeHeight);
                Assert.IsTrue(code39Command.PrintInterpretationLine);
                Assert.IsFalse(code39Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = CommandBase.ParseCommand("^B3");
            Assert.IsTrue(command is Code39BarCodeCommand);
            if (command is Code39BarCodeCommand code39Command)
            {
                Assert.AreEqual(Orientation.Normal, code39Command.Orientation);
                Assert.IsFalse(code39Command.Mod43CheckDigit);
                Assert.IsNull(code39Command.BarCodeHeight);
                Assert.IsTrue(code39Command.PrintInterpretationLine);
                Assert.IsFalse(code39Command.PrintInterpretationLineAboveCode);
            }
        }

    }
}
