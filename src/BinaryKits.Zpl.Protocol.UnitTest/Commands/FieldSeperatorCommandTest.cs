using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldSeperatorCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new FieldSeperatorCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FS", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new FieldSeperatorCommand();
            var isParsable = command.IsCommandParsable("^FS");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new FieldSeperatorCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new FieldSeperatorCommand();
            command.ParseCommand("^FS");
        }
    }
}
