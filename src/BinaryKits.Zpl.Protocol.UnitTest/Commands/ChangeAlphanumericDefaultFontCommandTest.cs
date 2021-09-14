using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class ChangeAlphanumericDefaultFontCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand('A');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^CFA,,", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand('A', 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^CFA,10,", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand('D', 400, 5000);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^CFD,400,5000", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidIndividualCharacterHeight_Exception()
        {
            new ChangeAlphanumericDefaultFontCommand('A', 600000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidIndividualCharacterWidth_Exception()
        {
            new ChangeAlphanumericDefaultFontCommand('A', 1, 600000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            var isParsable = command.IsCommandParsable("^CFA,9,5");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            command.ParseCommand("^CFA,,");
            Assert.AreEqual('A', command.SpecifiedDefaultFont);
            Assert.IsNull(command.IndividualCharacterHeight);
            Assert.IsNull(command.IndividualCharacterWidth);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            command.ParseCommand("^CFB,10,20");
            Assert.AreEqual('B', command.SpecifiedDefaultFont);
            Assert.AreEqual(10, command.IndividualCharacterHeight);
            Assert.AreEqual(20, command.IndividualCharacterWidth);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            command.ParseCommand("^CFB,,20");
            Assert.AreEqual('B', command.SpecifiedDefaultFont);
            Assert.IsNull(command.IndividualCharacterHeight);
            Assert.AreEqual(20, command.IndividualCharacterWidth);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            command.ParseCommand("^CFB,500,");
            Assert.AreEqual('B', command.SpecifiedDefaultFont);
            Assert.AreEqual(500, command.IndividualCharacterHeight);
            Assert.IsNull(command.IndividualCharacterWidth);
        }
    }
}
