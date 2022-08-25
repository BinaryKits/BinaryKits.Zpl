using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class FieldBlockCommandTest
    {
        [TestMethod]
        public void ToZpl_Default1_Successful()
        {
            var command = new FieldBlockCommand();
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FB0,1,0,L,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Default2_Successful()
        {
            var command = new FieldBlockCommand(10, 2, -5, TextJustification.Right, 5);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^FB10,2,-5,R,5", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_MaximumNumberOfLinesInTextBlockMin_Exception()
        {
            new FieldBlockCommand(maximumNumberOfLinesInTextBlock: 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_MaximumNumberOfLinesInTextBlockMax_Exception()
        {
            new FieldBlockCommand(maximumNumberOfLinesInTextBlock: 10000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_AddOrDeleteSpaceBetweenLinesMin_Exception()
        {
            new FieldBlockCommand(addOrDeleteSpaceBetweenLines: -10000);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_AddOrDeleteSpaceBetweenLinesMax_Exception()
        {
            new FieldBlockCommand(addOrDeleteSpaceBetweenLines: 10000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = FieldBlockCommand.CanParseCommand("^FB0,1,0,L,0");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = FieldBlockCommand.CanParseCommand("^FO10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^FB400,3,-2,C,4");
            Assert.IsTrue(command is FieldBlockCommand);
            if (command is FieldBlockCommand blockCommand)
            {
                Assert.AreEqual(400, blockCommand.WidthOfTextBlockLine);
                Assert.AreEqual(3, blockCommand.MaximumNumberOfLinesInTextBlock);
                Assert.AreEqual(-2, blockCommand.AddOrDeleteSpaceBetweenLines);
                Assert.AreEqual(TextJustification.Center, blockCommand.TextJustification);
                Assert.AreEqual(4, blockCommand.HangingIndentOfTheSecondAndRemainingLines);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^FB500");
            Assert.IsTrue(command is FieldBlockCommand);
            if (command is FieldBlockCommand blockCommand)
            {
                Assert.AreEqual(500, blockCommand.WidthOfTextBlockLine);
                Assert.AreEqual(1, blockCommand.MaximumNumberOfLinesInTextBlock);
                Assert.AreEqual(0, blockCommand.AddOrDeleteSpaceBetweenLines);
                Assert.AreEqual(TextJustification.Left, blockCommand.TextJustification);
                Assert.AreEqual(0, blockCommand.HangingIndentOfTheSecondAndRemainingLines);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^FB0,3,33");
            Assert.IsTrue(command is FieldBlockCommand);
            if (command is FieldBlockCommand blockCommand)
            {
                Assert.AreEqual(0, blockCommand.WidthOfTextBlockLine);
                Assert.AreEqual(3, blockCommand.MaximumNumberOfLinesInTextBlock);
                Assert.AreEqual(33, blockCommand.AddOrDeleteSpaceBetweenLines);
                Assert.AreEqual(TextJustification.Left, blockCommand.TextJustification);
                Assert.AreEqual(0, blockCommand.HangingIndentOfTheSecondAndRemainingLines);
            }
        }

    }
}
