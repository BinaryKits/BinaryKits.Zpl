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
            var command = new FieldDataCommand();
            var isParsable = command.IsCommandParsable("^FDTest");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new FieldDataCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new FieldDataCommand();
            command.ParseCommand("^FD");
            Assert.AreEqual(string.Empty, command.Data);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new FieldDataCommand();
            command.ParseCommand("^FDtest");
            Assert.AreEqual("test", command.Data);
        }
    }
}
