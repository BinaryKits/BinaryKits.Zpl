using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class StartFormatCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new StartFormatCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XA", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = StartFormatCommand.CanParseCommand("^XA");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = StartFormatCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^XA");
            Assert.IsTrue(command is StartFormatCommand);
        }

    }
}
