using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldDataCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new FieldDataCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FD", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaultWithData_Successful()
        {
            var command = new FieldDataCommand("Test");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FDTest", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = FieldDataCommand.CanParseCommand("^FDTest");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = FieldDataCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FD");
            Assert.IsTrue(command is FieldDataCommand);
            if (command is FieldDataCommand dataCommand)
            {
                Assert.AreEqual(string.Empty, dataCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FDtest");
            Assert.IsTrue(command is FieldDataCommand);
            if (command is FieldDataCommand dataCommand)
            {
                Assert.AreEqual("test", dataCommand.Data);
            }
        }

    }
}
