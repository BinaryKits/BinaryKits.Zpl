using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryKits.Zpl.Viewer.UnitTest
{
    [TestClass]
    public class CustomTest
    {
        [TestMethod]
        public void Custom()
        {
            string zplString = Common.LoadZPL("custom");
            Common.DefaultPrint(zplString, "custom.png");
        }
    }
}
