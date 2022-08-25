using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class CommentCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new CommentCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FX", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaultWithComment_Successful()
        {
            var command = new CommentCommand("Test");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FXTest", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = CommentCommand.CanParseCommand("^FXTest");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = CommentCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FX");
            Assert.IsTrue(command is CommentCommand);
            if (command is CommentCommand commentCommand)
            {
                Assert.AreEqual(string.Empty, commentCommand.NonPrintingComment);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FXtest");
            Assert.IsTrue(command is CommentCommand);
            if (command is CommentCommand commentCommand)
            {
                Assert.AreEqual("test", commentCommand.NonPrintingComment);
            }
        }

    }
}
