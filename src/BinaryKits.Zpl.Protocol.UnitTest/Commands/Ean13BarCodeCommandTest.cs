using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class Ean13BarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new Ean13BarCodeCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BEN,,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new Ean13BarCodeCommand(Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BEI,,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new Ean13BarCodeCommand(barCodeHeight: 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BEN,10,Y,N", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default4_Successful()
        {
            var command = new Ean13BarCodeCommand(printInterpretationLine: false, printInterpretationLineAboveCode: false);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BEN,,N,N", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight1_Exception()
        {
            new Ean13BarCodeCommand(barCodeHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBarCodeHeight2_Exception()
        {
            new Ean13BarCodeCommand(barCodeHeight: 33000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = Ean13BarCodeCommand.CanParseCommand("^BEN,,N,N");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = Ean13BarCodeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^BEN,,N,N");
            Assert.IsTrue(command is Ean13BarCodeCommand);
            if (command is Ean13BarCodeCommand code128Command)
            {
                Assert.AreEqual(Orientation.Normal, code128Command.Orientation);
                Assert.IsNull(code128Command.BarCodeHeight);
                Assert.IsFalse(code128Command.PrintInterpretationLine);
                Assert.IsFalse(code128Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^BEN,,Y,Y");
            Assert.IsTrue(command is Ean13BarCodeCommand);
            if (command is Ean13BarCodeCommand code128Command)
            {
                Assert.AreEqual(Orientation.Normal, code128Command.Orientation);
                Assert.IsNull(code128Command.BarCodeHeight);
                Assert.IsTrue(code128Command.PrintInterpretationLine);
                Assert.IsTrue(code128Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^BER,20,N,N");
            Assert.IsTrue(command is Ean13BarCodeCommand);
            if (command is Ean13BarCodeCommand code128Command)
            {
                Assert.AreEqual(Orientation.Rotated90, code128Command.Orientation);
                Assert.AreEqual(20, code128Command.BarCodeHeight);
                Assert.IsFalse(code128Command.PrintInterpretationLine);
                Assert.IsFalse(code128Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^BEB,55");
            Assert.IsTrue(command is Ean13BarCodeCommand);
            if (command is Ean13BarCodeCommand code128Command)
            {
                Assert.AreEqual(Orientation.Rotated270, code128Command.Orientation);
                Assert.AreEqual(55, code128Command.BarCodeHeight);
                Assert.IsTrue(code128Command.PrintInterpretationLine);
                Assert.IsFalse(code128Command.PrintInterpretationLineAboveCode);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = CommandBase.ParseCommand("^BE");
            Assert.IsTrue(command is Ean13BarCodeCommand);
            if (command is Ean13BarCodeCommand code128Command)
            {
                Assert.AreEqual(Orientation.Normal, code128Command.Orientation);
                Assert.IsNull(code128Command.BarCodeHeight);
                Assert.IsTrue(code128Command.PrintInterpretationLine);
                Assert.IsFalse(code128Command.PrintInterpretationLineAboveCode);
            }
        }

    }
}
