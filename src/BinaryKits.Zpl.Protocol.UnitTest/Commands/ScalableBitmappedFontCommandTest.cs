using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class ScalableBitmappedFontCommandTest
    {
        [TestMethod]
        public void ToZpl_Font0_Successful()
        {
            var command = new ScalableBitmappedFontCommand('0');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^A0N,10,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Font1_Successful()
        {
            var command = new ScalableBitmappedFontCommand('1');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^A1N,10,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_FontA_Successful()
        {
            var command = new ScalableBitmappedFontCommand('A');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^AAN,10,10", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Font0Rotated180_Successful()
        {
            var command = new ScalableBitmappedFontCommand('0', Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^A0I,10,10", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCharacterHeight_Exception()
        {
            new ScalableBitmappedFontCommand('A', Orientation.Normal, characterHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidWidth_Exception()
        {
            new ScalableBitmappedFontCommand('A', Orientation.Normal, width: 0);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = ScalableBitmappedFontCommand.CanParseCommand("^AAN10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = ScalableBitmappedFontCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^AA");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('A', fontCommand.FontName);
                Assert.AreEqual(Orientation.Normal, fontCommand.Orientation);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^A0");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Normal, fontCommand.Orientation);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^A0N");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Normal, fontCommand.Orientation);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^A0I");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated180, fontCommand.Orientation);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = CommandBase.ParseCommand("^A0R");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated90, fontCommand.Orientation);
            }
        }

        [TestMethod]
        public void ParseCommand_InvalidFieldOrientationCommand_Successful()
        {
            var command = CommandBase.ParseCommand("^A0X");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Normal, fontCommand.Orientation);
                Assert.IsNull(fontCommand.CharacterHeight);
                Assert.IsNull(fontCommand.Width);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = CommandBase.ParseCommand("^A0B,10");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated270, fontCommand.Orientation);
                Assert.AreEqual(10, fontCommand.CharacterHeight);
                Assert.IsNull(fontCommand.Width);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand7_Successful()
        {
            var command = CommandBase.ParseCommand("^A0I,20");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated180, fontCommand.Orientation);
                Assert.AreEqual(20, fontCommand.CharacterHeight);
                Assert.IsNull(fontCommand.Width);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand8_Successful()
        {
            var command = CommandBase.ParseCommand("^A0I,,20");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated180, fontCommand.Orientation);
                Assert.IsNull(fontCommand.CharacterHeight);
                Assert.AreEqual(20, fontCommand.Width);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand9_Successful()
        {
            var command = CommandBase.ParseCommand("^A0I,100,50");
            Assert.IsTrue(command is ScalableBitmappedFontCommand);
            if (command is ScalableBitmappedFontCommand fontCommand)
            {
                Assert.AreEqual('0', fontCommand.FontName);
                Assert.AreEqual(Orientation.Rotated180, fontCommand.Orientation);
                Assert.AreEqual(100, fontCommand.CharacterHeight);
                Assert.AreEqual(50, fontCommand.Width);
            }
        }
    }
}
