using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class RecallGraphicCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new RecallGraphicCommand("R:", "TEST.GRF");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XGR:TEST.GRF,1,1", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new RecallGraphicCommand("R:", "TEST.PNG");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XGR:TEST.PNG,1,1", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new RecallGraphicCommand("R:", "TEST.PNG", 2, 2);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^XGR:TEST.PNG,2,2", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var command = new RecallGraphicCommand();
            var isParsable = command.IsCommandParsable("^XGR:TEST.PNG");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new RecallGraphicCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new RecallGraphicCommand();
            command.ParseCommand("^XGR:TEST.PNG");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("TEST.PNG", command.ImageName);
            Assert.AreEqual(1, command.MagnificationFactorX);
            Assert.AreEqual(1, command.MagnificationFactorY);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new RecallGraphicCommand();
            command.ParseCommand("^XGR:IMAGE.GRF,2");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("IMAGE.GRF", command.ImageName);
            Assert.AreEqual(2, command.MagnificationFactorX);
            Assert.AreEqual(1, command.MagnificationFactorY);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new RecallGraphicCommand();
            command.ParseCommand("^XGR:");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("UNKNOWN.GRF", command.ImageName);
            Assert.AreEqual(1, command.MagnificationFactorX);
            Assert.AreEqual(1, command.MagnificationFactorY);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new RecallGraphicCommand();
            command.ParseCommand("^XGR:IMAGE.GRF,3,3");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("IMAGE.GRF", command.ImageName);
            Assert.AreEqual(3, command.MagnificationFactorX);
            Assert.AreEqual(3, command.MagnificationFactorY);
        }
    }
}
