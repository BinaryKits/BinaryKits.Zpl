using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class LabelReversePrintCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new LabelReversePrintCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LRN", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new LabelReversePrintCommand(true);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^LRY", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = LabelReversePrintCommand.CanParseCommand("^LRY");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = LabelReversePrintCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^LR");
            Assert.IsTrue(command is LabelReversePrintCommand);
            if (command is LabelReversePrintCommand reversePrintCommand)
            {
                Assert.IsFalse(reversePrintCommand.ReversePrintAllFields);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^LRN");
            Assert.IsTrue(command is LabelReversePrintCommand);
            if (command is LabelReversePrintCommand reversePrintCommand)
            {
                Assert.IsFalse(reversePrintCommand.ReversePrintAllFields);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^LRY");
            Assert.IsTrue(command is LabelReversePrintCommand);
            if (command is LabelReversePrintCommand reversePrintCommand)
            {
                Assert.IsTrue(reversePrintCommand.ReversePrintAllFields);
            }
        }

    }
}
