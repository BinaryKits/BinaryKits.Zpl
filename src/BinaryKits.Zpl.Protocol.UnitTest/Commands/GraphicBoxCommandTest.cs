using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class GraphicBoxCommandTest
    {
        [TestMethod]
        public void ToZpl_Width10Height10_Successful()
        {
            var command = new GraphicBoxCommand(10, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GB10,10,1,B,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_Width1Height1_Successful()
        {
            var command = new GraphicBoxCommand(1, 1);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GB1,1,1,B,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_WidthNullHeight10_Successful()
        {
            var command = new GraphicBoxCommand(null, 10);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GB1,10,1,B,0", zplCommand);
        }

        [TestMethod]
        public void ToZpl_WidthNullHeightNull_Successful()
        {
            var command = new GraphicBoxCommand(null, null);
            var zplCommand = command.ToZpl();
            Assert.AreEqual("^GB1,1,1,B,0", zplCommand);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Must be between 1 and 32000")]
        public void Constructor_WidthMinus10Height1_Exception()
        {
            new GraphicBoxCommand(-10, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Must be between 1 and 32000")]
        public void Constructor_Widht1HeightMinus10_Exception()
        {
            new GraphicBoxCommand(1, -10);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand1_True()
        {
            var command = new GraphicBoxCommand();
            var isParsable = command.IsCommandParsable("^GB10,10");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand2_True()
        {
            var command = new GraphicBoxCommand();
            var isParsable = command.IsCommandParsable("^GB10,10,1,W,1");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new GraphicBoxCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new GraphicBoxCommand();
            command.ParseCommand("^GB10,10");
            Assert.AreEqual(command.BoxWidth, 10);
            Assert.AreEqual(command.BoxHeight, 10);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new GraphicBoxCommand();
            command.ParseCommand("^GB10,10,2,W,1");
            Assert.AreEqual(command.BoxWidth, 10);
            Assert.AreEqual(command.BoxHeight, 10);
            Assert.AreEqual(command.BorderThickness, 2);
            Assert.AreEqual(command.LineColor, LineColor.White);
            Assert.AreEqual(command.DegreeOfCornerrounding, 1);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand3_Successful()
        {
            var command = new GraphicBoxCommand();
            command.ParseCommand("^GB10,10,2,,2");
            Assert.AreEqual(command.BoxWidth, 10);
            Assert.AreEqual(command.BoxHeight, 10);
            Assert.AreEqual(command.BorderThickness, 2);
            Assert.AreEqual(command.LineColor, LineColor.Black);
            Assert.AreEqual(command.DegreeOfCornerrounding, 2);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand4_Successful()
        {
            var command = new GraphicBoxCommand();
            command.ParseCommand("^GB10,10,20");
            Assert.AreEqual(command.BoxWidth, 20);
            Assert.AreEqual(command.BoxHeight, 20);
            Assert.AreEqual(command.BorderThickness, 20);
        }
    }
}
