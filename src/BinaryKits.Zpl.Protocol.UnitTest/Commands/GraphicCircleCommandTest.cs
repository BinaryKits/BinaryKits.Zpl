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
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new GraphicCircleCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new GraphicCircleCommand();
            command.ParseCommand("^GC10,5");
            Assert.AreEqual(10, command.CircleDiameter);
            Assert.AreEqual(5, command.BorderThickness);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new GraphicCircleCommand();
            command.ParseCommand("^GC,2,W");
            Assert.AreEqual(3, command.CircleDiameter);
            Assert.AreEqual(2, command.BorderThickness);
            Assert.AreEqual(LineColor.White, command.LineColor);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new GraphicCircleCommand();
            command.ParseCommand("^GC,,W");
            Assert.AreEqual(3, command.CircleDiameter);
            Assert.AreEqual(1, command.BorderThickness);
            Assert.AreEqual(LineColor.White, command.LineColor);
        }
    }
}
