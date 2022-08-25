using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class QrCodeBarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new QrCodeBarCodeCommand(Orientation.Normal, 2, 1, ErrorCorrectionLevel.HighReliability, 7);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BQN,2,1,Q,7", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = QrCodeBarCodeCommand.CanParseCommand("^BQN,2,1,Q,7");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = QrCodeBarCodeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^BQN");
            Assert.IsTrue(command is QrCodeBarCodeCommand);
            if (command is QrCodeBarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.AreEqual(2, barCodeCommand.Model);
                Assert.AreEqual(1, barCodeCommand.MagnificationFactor);
                Assert.AreEqual(ErrorCorrectionLevel.HighReliability, barCodeCommand.ErrorCorrection);
                Assert.AreEqual(7, barCodeCommand.MaskValue);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^BQN,2,1,Q,7");
            Assert.IsTrue(command is QrCodeBarCodeCommand);
            if (command is QrCodeBarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.AreEqual(2, barCodeCommand.Model);
                Assert.AreEqual(1, barCodeCommand.MagnificationFactor);
                Assert.AreEqual(ErrorCorrectionLevel.HighReliability, barCodeCommand.ErrorCorrection);
                Assert.AreEqual(7, barCodeCommand.MaskValue);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^BQN,1,2,M,1");
            Assert.IsTrue(command is QrCodeBarCodeCommand);
            if (command is QrCodeBarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.AreEqual(1, barCodeCommand.Model);
                Assert.AreEqual(2, barCodeCommand.MagnificationFactor);
                Assert.AreEqual(ErrorCorrectionLevel.Standard, barCodeCommand.ErrorCorrection);
                Assert.AreEqual(1, barCodeCommand.MaskValue);
            }
        }

        [TestMethod]
        public void ParseCommand_InvalidCommandSetDefaults_Successful()
        {
            var command = CommandBase.ParseCommand("^BQX,1,2,X,1");
            Assert.IsTrue(command is QrCodeBarCodeCommand);
            if (command is QrCodeBarCodeCommand barCodeCommand)
            {
                Assert.AreEqual(Orientation.Normal, barCodeCommand.Orientation);
                Assert.AreEqual(1, barCodeCommand.Model);
                Assert.AreEqual(2, barCodeCommand.MagnificationFactor);
                Assert.AreEqual(ErrorCorrectionLevel.HighReliability, barCodeCommand.ErrorCorrection);
                Assert.AreEqual(1, barCodeCommand.MaskValue);
            }
        }
    }
}
