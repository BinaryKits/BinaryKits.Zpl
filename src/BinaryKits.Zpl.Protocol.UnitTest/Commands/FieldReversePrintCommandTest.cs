using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldReversePrintCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new FieldReversePrintCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FR", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new FieldReversePrintCommand();
            var isParsable = command.IsCommandParsable("^FR");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new FieldReversePrintCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new FieldReversePrintCommand();
            command.ParseCommand("^FR");
        }
    }
}
