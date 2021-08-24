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
        [ExpectedException(typeof(ArgumentException), "Must be between 10 and 32000")]
        public void Constructor_InvalidCharacterHeight_Exception()
        {
            new ScalableBitmappedFontCommand('A', Orientation.Normal, characterHeight: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Must be between 10 and 32000")]
        public void Constructor_InvalidWidth_Exception()
        {
            new ScalableBitmappedFontCommand('A', Orientation.Normal, width: 0);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new ScalableBitmappedFontCommand();
            var isParsable = command.IsCommandParsable("^AAN10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new ScalableBitmappedFontCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^AA");
            Assert.AreEqual(command.FontName, 'A');
            Assert.AreEqual(command.Orientation, Orientation.Normal);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Normal);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0N");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Normal);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0I");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated180);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0R");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated90);
        }

        [TestMethod]
        public void ParseCommand_InvalidFieldOrientationCommand_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0X");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Normal);
            Assert.IsNull(command.CharacterHeight);
            Assert.IsNull(command.Width);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0B,10");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated270);
            Assert.AreEqual(command.CharacterHeight, 10);
            Assert.IsNull(command.Width);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand7_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0I,20");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated180);
            Assert.AreEqual(command.CharacterHeight, 20);
            Assert.IsNull(command.Width);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand8_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0I,,20");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated180);
            Assert.IsNull(command.CharacterHeight);
            Assert.AreEqual(command.Width, 20);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand9_Successful()
        {
            var command = new ScalableBitmappedFontCommand();
            command.ParseCommand("^A0I,100,50");
            Assert.AreEqual(command.FontName, '0');
            Assert.AreEqual(command.Orientation, Orientation.Rotated180);
            Assert.AreEqual(command.CharacterHeight, 100);
            Assert.AreEqual(command.Width, 50);
        }
    }
}
