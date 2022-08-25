using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class EndFormatCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new EndFormatCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XZ", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = EndFormatCommand.CanParseCommand("^XZ");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = EndFormatCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^XZ");
            Assert.IsTrue(command is EndFormatCommand);
        }

    }
}
