using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Protocol.Commands.UnitTest
{
    [TestClass]
    public class DownloadObjectsCommandTest
    {
        [TestMethod]
        public void ToZpl_Default_Successful()
        {
            var command = new DownloadObjectsCommand("R:", "TEST.PNG", 'P', "P", 4, 2, "ABCDEF00");
            var zplCommand = command.ToZpl();
            Assert.AreEqual("~DYR:TEST.PNG,P,P,4,2,ABCDEF00", zplCommand);
        }

        [TestMethod]
        public void IsCommandParsable_ValidCommand_True()
        {
            var isParsable = DownloadObjectsCommand.CanParseCommand("~DYR:TEST.PNG,P,P,4,2,ABCDEF00");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var isParsable = DownloadObjectsCommand.CanParseCommand("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("~DYR:TEST.PNG,P,P,4,2,ABCDEF00");
            Assert.IsTrue(command is DownloadObjectsCommand);
            if (command is DownloadObjectsCommand objectsCommand)
            {
                Assert.AreEqual("R:", objectsCommand.StorageDevice);
                Assert.AreEqual("TEST.PNG", objectsCommand.FileName);
                Assert.AreEqual('P', objectsCommand.FormatDownloadedInDataField);
                Assert.AreEqual("P", objectsCommand.ExtensionOfStoredFile);
                Assert.AreEqual(4, objectsCommand.TotalNumberOfBytesInFile);
                Assert.AreEqual(2, objectsCommand.TotalNumberOfBytesPerRow);
                Assert.AreEqual("ABCDEF00", objectsCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("~DYR:,P,P,4,2,ABCDEF00");
            Assert.IsTrue(command is DownloadObjectsCommand);
            if (command is DownloadObjectsCommand objectsCommand)
            {
                Assert.AreEqual("R:", objectsCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN", objectsCommand.FileName);
                Assert.AreEqual('P', objectsCommand.FormatDownloadedInDataField);
                Assert.AreEqual("P", objectsCommand.ExtensionOfStoredFile);
                Assert.AreEqual(4, objectsCommand.TotalNumberOfBytesInFile);
                Assert.AreEqual(2, objectsCommand.TotalNumberOfBytesPerRow);
                Assert.AreEqual("ABCDEF00", objectsCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_InvalidCommand1_Successful()
        {
            var command = CommandBase.ParseCommand("~DYIMAGE2.PNG,P,P,4,2");
            Assert.IsTrue(command is DownloadObjectsCommand);
            if (command is DownloadObjectsCommand objectsCommand)
            {
                Assert.AreEqual("R:", objectsCommand.StorageDevice);
                Assert.AreEqual("IMAGE2.PNG", objectsCommand.FileName);
                Assert.AreEqual('P', objectsCommand.FormatDownloadedInDataField);
                Assert.AreEqual("P", objectsCommand.ExtensionOfStoredFile);
                Assert.AreEqual(4, objectsCommand.TotalNumberOfBytesInFile);
                Assert.AreEqual(2, objectsCommand.TotalNumberOfBytesPerRow);
                Assert.AreEqual(null, objectsCommand.Data);
            }
        }

        [TestMethod]
        public void ParseCommand_InvalidCommand2_Successful()
        {
            var command = CommandBase.ParseCommand("~DYR:");
            Assert.IsTrue(command is DownloadObjectsCommand);
            if (command is DownloadObjectsCommand objectsCommand)
            {
                Assert.AreEqual("R:", objectsCommand.StorageDevice);
                Assert.AreEqual("UNKNOWN", objectsCommand.FileName);
                Assert.AreEqual(char.MinValue, objectsCommand.FormatDownloadedInDataField);
                Assert.AreEqual(null, objectsCommand.ExtensionOfStoredFile);
                Assert.AreEqual(0, objectsCommand.TotalNumberOfBytesInFile);
                Assert.AreEqual(0, objectsCommand.TotalNumberOfBytesPerRow);
                Assert.AreEqual(null, objectsCommand.Data);
            }
        }

    }
}
