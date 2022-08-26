using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class DataMatrixBarCodeCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new DataMatrixBarCodeCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BXN,,0,,,6,~,1", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new DataMatrixBarCodeCommand(Orientation.Rotated180);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BXI,,0,,,6,~,1", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default3_Successful()
        {
            var command = new DataMatrixBarCodeCommand(elementHeight: 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BXN,10,0,,,6,~,1", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default4_Successful()
        {
            var command = new DataMatrixBarCodeCommand(qualityLevel: 200, columnsToEncode: 10, rowsToEncode: 10, formatId: null, escapeSequenceControlChar: '\\');
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^BXN,,200,10,10,,\\,1", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = DataMatrixBarCodeCommand.CanParseCommand("^BXN,,200");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = DataMatrixBarCodeCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^BXN,,200");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Normal, dataMatrixCommand.Orientation);
                Assert.IsNull(dataMatrixCommand.ElementHeight);
                Assert.AreEqual(200, dataMatrixCommand.QualityLevel);
                Assert.IsNull(dataMatrixCommand.ColumnsToEncode);
                Assert.IsNull(dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(6, dataMatrixCommand.FormatId);
                Assert.AreEqual('~', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(1, dataMatrixCommand.AspectRatio);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^BX,10,140,,,,,2");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Normal, dataMatrixCommand.Orientation);
                Assert.AreEqual(10, dataMatrixCommand.ElementHeight);
                Assert.AreEqual(140, dataMatrixCommand.QualityLevel);
                Assert.IsNull(dataMatrixCommand.ColumnsToEncode);
                Assert.IsNull(dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(6, dataMatrixCommand.FormatId);
                Assert.AreEqual('~', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(2, dataMatrixCommand.AspectRatio);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^BXI,,200,10,10");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Rotated180, dataMatrixCommand.Orientation);
                Assert.IsNull(dataMatrixCommand.ElementHeight);
                Assert.AreEqual(200, dataMatrixCommand.QualityLevel);
                Assert.AreEqual(10, dataMatrixCommand.ColumnsToEncode);
                Assert.AreEqual(10, dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(6, dataMatrixCommand.FormatId);
                Assert.AreEqual('~', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(1, dataMatrixCommand.AspectRatio);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = CommandBase.ParseCommand("^BXR,8,,9,9");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Rotated90, dataMatrixCommand.Orientation);
                Assert.AreEqual(8, dataMatrixCommand.ElementHeight);
                Assert.AreEqual(0, dataMatrixCommand.QualityLevel);
                Assert.AreEqual(9, dataMatrixCommand.ColumnsToEncode);
                Assert.AreEqual(9, dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(6, dataMatrixCommand.FormatId);
                Assert.AreEqual('~', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(1, dataMatrixCommand.AspectRatio);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand5_Successful()
        {
            var command = CommandBase.ParseCommand("^BXB,10,100,11,11,5,_");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Rotated270, dataMatrixCommand.Orientation);
                Assert.AreEqual(10, dataMatrixCommand.ElementHeight);
                Assert.AreEqual(100, dataMatrixCommand.QualityLevel);
                Assert.AreEqual(11, dataMatrixCommand.ColumnsToEncode);
                Assert.AreEqual(11, dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(5, dataMatrixCommand.FormatId);
                Assert.AreEqual('_', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(1, dataMatrixCommand.AspectRatio);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand6_Successful()
        {
            var command = CommandBase.ParseCommand("^BX");
            Assert.IsTrue(command is DataMatrixBarCodeCommand);
            if (command is DataMatrixBarCodeCommand dataMatrixCommand)
            {
                Assert.AreEqual(Orientation.Normal, dataMatrixCommand.Orientation);
                Assert.IsNull(dataMatrixCommand.ElementHeight);
                Assert.AreEqual(0, dataMatrixCommand.QualityLevel);
                Assert.IsNull(dataMatrixCommand.ColumnsToEncode);
                Assert.IsNull(dataMatrixCommand.RowsToEncode);
                Assert.AreEqual(6, dataMatrixCommand.FormatId);
                Assert.AreEqual('~', dataMatrixCommand.EscapeSequenceControlChar);
                Assert.AreEqual(1, dataMatrixCommand.AspectRatio);
            }
        }

    }
}
