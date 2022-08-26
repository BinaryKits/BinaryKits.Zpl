using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldHexadecimalIndicatorCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new FieldHexadecimalIndicatorCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FH_", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaultWithData_Successful()
        {
            var command = new FieldHexadecimalIndicatorCommand('\\');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FH\\", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = FieldHexadecimalIndicatorCommand.CanParseCommand("^FH");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = FieldHexadecimalIndicatorCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FH");
            Assert.IsTrue(command is FieldHexadecimalIndicatorCommand);
            if (command is FieldHexadecimalIndicatorCommand hexadecimalCommand)
            {
                Assert.AreEqual('_', hexadecimalCommand.HexidecimalIndicator);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FH`");
            Assert.IsTrue(command is FieldHexadecimalIndicatorCommand);
            if (command is FieldHexadecimalIndicatorCommand hexadecimalCommand)
            {
                Assert.AreEqual('`', hexadecimalCommand.HexidecimalIndicator);
            }
        }

    }
}
