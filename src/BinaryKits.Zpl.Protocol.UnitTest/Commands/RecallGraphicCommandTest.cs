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
            var isParsable = RecallGraphicCommand.CanParseCommand("^XGR:TEST.PNG");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = RecallGraphicCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^XGR:TEST.PNG");
            Assert.IsTrue(command is RecallGraphicCommand);
            if (command is RecallGraphicCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("TEST.PNG", recallCommand.ImageName);
                Assert.AreEqual(1, recallCommand.MagnificationFactorX);
                Assert.AreEqual(1, recallCommand.MagnificationFactorY);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^XGIMAGE.GRF,2");
            Assert.IsTrue(command is RecallGraphicCommand);
            if (command is RecallGraphicCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("IMAGE.GRF", recallCommand.ImageName);
                Assert.AreEqual(2, recallCommand.MagnificationFactorX);
                Assert.AreEqual(1, recallCommand.MagnificationFactorY);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^XGR:");
            Assert.IsTrue(command is RecallGraphicCommand);
            if (command is RecallGraphicCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN.GRF", recallCommand.ImageName);
                Assert.AreEqual(1, recallCommand.MagnificationFactorX);
                Assert.AreEqual(1, recallCommand.MagnificationFactorY);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^XGR:IMAGE.GRF,3,3");
            Assert.IsTrue(command is RecallGraphicCommand);
            if (command is RecallGraphicCommand recallCommand)
            {
                Assert.AreEqual("R:", recallCommand.StorageDevice);
                Assert.AreEqual("IMAGE.GRF", recallCommand.ImageName);
                Assert.AreEqual(3, recallCommand.MagnificationFactorX);
                Assert.AreEqual(3, recallCommand.MagnificationFactorY);
            }
        }

    }
}
