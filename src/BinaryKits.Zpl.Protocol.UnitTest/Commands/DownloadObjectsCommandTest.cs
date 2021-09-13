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
            var command = new DownloadObjectsCommand();
            var isParsable = command.IsCommandParsable("~DYR:TEST.PNG,P,P,4,2,ABCDEF00");
            Assert.IsTrue(isParsable);
        }

        [TestMethod]
        public void IsCommandParsable_InvalidCommand_False()
        {
            var command = new DownloadObjectsCommand();
            var isParsable = command.IsCommandParsable("^FT10,10");
            Assert.IsFalse(isParsable);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand1_Successful()
        {
            var command = new DownloadObjectsCommand();
            command.ParseCommand("~DYR:TEST.PNG,P,P,4,2,ABCDEF00");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("TEST.PNG", command.FileName);
            Assert.AreEqual('P', command.FormatDownloadedInDataField);
            Assert.AreEqual("P", command.ExtensionOfStoredFile);
            Assert.AreEqual(4, command.TotalNumberOfBytesInFile);
            Assert.AreEqual(2, command.TotalNumberOfBytesPerRow);
            Assert.AreEqual("ABCDEF00", command.Data);
        }

        [TestMethod]
        public void ParseCommand_ValidCommand2_Successful()
        {
            var command = new DownloadObjectsCommand();
            command.ParseCommand("~DYR:,P,P,4,2,ABCDEF00");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("UNKNOWN", command.FileName);
            Assert.AreEqual('P', command.FormatDownloadedInDataField);
            Assert.AreEqual("P", command.ExtensionOfStoredFile);
            Assert.AreEqual(4, command.TotalNumberOfBytesInFile);
            Assert.AreEqual(2, command.TotalNumberOfBytesPerRow);
            Assert.AreEqual("ABCDEF00", command.Data);
        }

        [TestMethod]
        public void ParseCommand_InvalidCommand1_Successful()
        {
            var command = new DownloadObjectsCommand();
            command.ParseCommand("~DYR:IMAGE2.PNG,P,P,4,2");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("IMAGE2.PNG", command.FileName);
            Assert.AreEqual('P', command.FormatDownloadedInDataField);
            Assert.AreEqual("P", command.ExtensionOfStoredFile);
            Assert.AreEqual(4, command.TotalNumberOfBytesInFile);
            Assert.AreEqual(2, command.TotalNumberOfBytesPerRow);
            Assert.AreEqual(null, command.Data);
        }

        [TestMethod]
        public void ParseCommand_InvalidCommand2_Successful()
        {
            var command = new DownloadObjectsCommand();
            command.ParseCommand("~DYR:");
            Assert.AreEqual("R:", command.StorageDevice);
            Assert.AreEqual("UNKNOWN", command.FileName);
            Assert.AreEqual(char.MinValue, command.FormatDownloadedInDataField);
            Assert.AreEqual(null, command.ExtensionOfStoredFile);
            Assert.AreEqual(0, command.TotalNumberOfBytesInFile);
            Assert.AreEqual(0, command.TotalNumberOfBytesPerRow);
            Assert.AreEqual(null, command.Data);
        }
    }
}
