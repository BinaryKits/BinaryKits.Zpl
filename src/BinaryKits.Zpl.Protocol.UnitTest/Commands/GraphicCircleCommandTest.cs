using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class GraphicCircleCommandTest
    {
        [TestMethod]
        public void ToZpl_DefaulValues1_Successful()
        {
            var command = new GraphicCircleCommand(10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GC10,1,B", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaulValues2_Successful()
        {
            var command = new GraphicCircleCommand(10, 5);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GC10,5,B", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaulValues3_Successful()
        {
            var command = new GraphicCircleCommand(10, 5, LineColor.White);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GC10,5,W", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaulValues4_Successful()
        {
            var command = new GraphicCircleCommand(null, 5, LineColor.White);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GC3,5,W", zplCommand);
        }

        [TestMethod]
        public void ToZpl_DefaulValues5_Successful()
        {
            var command = new GraphicCircleCommand(null, null, LineColor.White);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GC3,1,W", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidCircleDiameter_Exception()
        {
            new GraphicCircleCommand(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_InvalidBorderThickness_Exception()
        {
            new GraphicCircleCommand(3, 5000);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = GraphicCircleCommand.CanParseCommand("^GC10,1,B");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = GraphicCircleCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("^GC10,5");
            Assert.IsTrue(command is GraphicCircleCommand);
            if (command is GraphicCircleCommand circleCommand)
            {
                Assert.AreEqual(10, circleCommand.CircleDiameter);
                Assert.AreEqual(5, circleCommand.BorderThickness);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("^GC,2,W");
            Assert.IsTrue(command is GraphicCircleCommand);
            if (command is GraphicCircleCommand circleCommand)
            {
                Assert.AreEqual(3, circleCommand.CircleDiameter);
                Assert.AreEqual(2, circleCommand.BorderThickness);
                Assert.AreEqual(LineColor.White, circleCommand.LineColor);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = CommandBase.ParseCommand("^GC,,W");
            Assert.IsTrue(command is GraphicCircleCommand);
            if (command is GraphicCircleCommand circleCommand)
            {
                Assert.AreEqual(3, circleCommand.CircleDiameter);
                Assert.AreEqual(1, circleCommand.BorderThickness);
                Assert.AreEqual(LineColor.White, circleCommand.LineColor);
            }
        }

    }
}
