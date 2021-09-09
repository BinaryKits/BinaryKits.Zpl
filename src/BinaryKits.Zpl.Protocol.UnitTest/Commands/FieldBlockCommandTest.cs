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
            var command = new FieldBlockCommand();
            var isParsable = command.IsCommandParsable("^FB0,1,0,L,0");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new FieldBlockCommand();
            var isParsable = command.IsCommandParsable("^FO10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new FieldBlockCommand();
            command.ParseCommand("^FB400,3,-2,C,4");
            Assert.AreEqual(400, command.WidthOfTextBlockLine);
            Assert.AreEqual(3, command.MaximumNumberOfLinesInTextBlock);
            Assert.AreEqual(-2, command.AddOrDeleteSpaceBetweenLines);
            Assert.AreEqual(TextJustification.Center, command.TextJustification);
            Assert.AreEqual(4, command.HangingIndentOfTheSecondAndRemainingLines);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new FieldBlockCommand();
            command.ParseCommand("^FB500");
            Assert.AreEqual(500, command.WidthOfTextBlockLine);
            Assert.AreEqual(1, command.MaximumNumberOfLinesInTextBlock);
            Assert.AreEqual(0, command.AddOrDeleteSpaceBetweenLines);
            Assert.AreEqual(TextJustification.Left, command.TextJustification);
            Assert.AreEqual(0, command.HangingIndentOfTheSecondAndRemainingLines);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new FieldBlockCommand();
            command.ParseCommand("^FB0,3,33");
            Assert.AreEqual(0, command.WidthOfTextBlockLine);
            Assert.AreEqual(3, command.MaximumNumberOfLinesInTextBlock);
            Assert.AreEqual(33, command.AddOrDeleteSpaceBetweenLines);
            Assert.AreEqual(TextJustification.Left, command.TextJustification);
            Assert.AreEqual(0, command.HangingIndentOfTheSecondAndRemainingLines);
        }
    }
}
