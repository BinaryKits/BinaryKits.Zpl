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
            var command = new QrCodeBarCodeCommand();
            var isParsable = command.IsCommandParsable("^BQN,2,1,Q,7");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new QrCodeBarCodeCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new QrCodeBarCodeCommand();
            command.ParseCommand("^BQN");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.AreEqual(2, command.Model);
            Assert.AreEqual(1, command.MagnificationFactor);
            Assert.AreEqual(ErrorCorrectionLevel.HighReliability, command.ErrorCorrection);
            Assert.AreEqual(7, command.MaskValue);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new QrCodeBarCodeCommand();
            command.ParseCommand("^BQN,2,1,Q,7");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.AreEqual(2, command.Model);
            Assert.AreEqual(1, command.MagnificationFactor);
            Assert.AreEqual(ErrorCorrectionLevel.HighReliability, command.ErrorCorrection);
            Assert.AreEqual(7, command.MaskValue);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new QrCodeBarCodeCommand();
            command.ParseCommand("^BQN,1,2,M,1");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.AreEqual(1, command.Model);
            Assert.AreEqual(2, command.MagnificationFactor);
            Assert.AreEqual(ErrorCorrectionLevel.Standard, command.ErrorCorrection);
            Assert.AreEqual(1, command.MaskValue);
        }

        [TestMethod]
        public void ParseCommand_InvalidCommandSetDefaults_Successful()
        {
            var command = new QrCodeBarCodeCommand();
            command.ParseCommand("^BQX,1,2,X,1");
            Assert.AreEqual(Orientation.Normal, command.Orientation);
            Assert.AreEqual(1, command.Model);
            Assert.AreEqual(2, command.MagnificationFactor);
            Assert.AreEqual(ErrorCorrectionLevel.HighReliability, command.ErrorCorrection);
            Assert.AreEqual(1, command.MaskValue);
        }
    }
}
