using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldNumberCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new FieldNumberCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FN0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaultWithNumber_Successful()
        {
            var command = new FieldNumberCommand(2);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FN2", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = FieldNumberCommand.CanParseCommand("^FN2\"Test\"");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = FieldNumberCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FN");
            Assert.IsTrue(command is FieldNumberCommand);
            if (command is FieldNumberCommand numberCommand)
            {
                Assert.AreEqual(0, numberCommand.AssignedNumber);
                Assert.AreEqual("optional parameter", numberCommand.PromptDisplay);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FN\"test\"");
            Assert.IsTrue(command is FieldNumberCommand);
            if (command is FieldNumberCommand numberCommand)
            {
                Assert.AreEqual(0, numberCommand.AssignedNumber);
                Assert.AreEqual("test", numberCommand.PromptDisplay);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^FN2");
            Assert.IsTrue(command is FieldNumberCommand);
            if (command is FieldNumberCommand numberCommand)
            {
                Assert.AreEqual(2, numberCommand.AssignedNumber);
                Assert.AreEqual("optional parameter", numberCommand.PromptDisplay);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^FN9999\"enter a number\"");
            Assert.IsTrue(command is FieldNumberCommand);
            if (command is FieldNumberCommand numberCommand)
            {
                Assert.AreEqual(9999, numberCommand.AssignedNumber);
                Assert.AreEqual("enter a number", numberCommand.PromptDisplay);
            }
        }

    }
}
