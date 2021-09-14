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
            var command = new CommentCommand();
            var isParsable = command.IsCommandParsable("^FXTest");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new CommentCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new CommentCommand();
            command.ParseCommand("^FX");
            Assert.AreEqual(string.Empty, command.NonPrintingComment);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new CommentCommand();
            command.ParseCommand("^FXtest");
            Assert.AreEqual("test", command.NonPrintingComment);
        }
    }
}
