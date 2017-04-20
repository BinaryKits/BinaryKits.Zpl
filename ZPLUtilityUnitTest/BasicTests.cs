using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BinaryKits.Utility.ZPLUtility;
using System.Collections.Generic;

namespace ZPLUtilityUnitTest
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void SingelElement()
        {
            var result = new ZPLGraphicBox(100, 100, 100, 100).ToZPLString();
            Console.WriteLine(result);
        }

        [TestMethod]
        public void MultipleElements()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            ZPLFont font = new ZPLFont(fontWidth: 50, fontHeight: 50);
            var labelElements = new List<ZPLElementBase>();
            labelElements.Add(new ZPLTextField(sampleText, 50, 100, font));
            labelElements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));
            labelElements.Add(new ZPLGraphicBox(450, 750, 100, 100, 50, ZPLConstants.LineColor.White));
            labelElements.Add(new ZPLGraphicCircle(400, 700, 100, 5));
            labelElements.Add(new ZPLGraphicDiagonalLine(400, 700, 100, 50, 5));
            labelElements.Add(new ZPLGraphicDiagonalLine(400, 700, 50, 100, 5));
            labelElements.Add(new ZPLGraphicSymbol(ZPLGraphicSymbol.GraphicSymbolCharacter.Copyright, 600, 600, 50, 50));

            //Add raw ZPL code
            labelElements.Add(new ZPLRaw("^FO200, 200^GB300, 200, 10 ^FS"));

            var renderEngine = new ZPLEngine(labelElements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void ChangeDPI()
        {
            var labelElements = new List<ZPLElementBase>();
            labelElements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));

            var options = new ZPLRenderOptions() { SourcePrintDPI = 203, TargetPrintDPI = 300 };
            var output = new ZPLEngine(labelElements).ToZPLString(options);

            Console.WriteLine(output);
        }
    }
}
