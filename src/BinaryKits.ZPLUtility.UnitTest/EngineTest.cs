using BinaryKits.ZPLUtility;
using BinaryKits.ZPLUtility.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace ZPLUtility.UnitTest
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void SingelElement()
        {
            var output = new ZPLGraphicBox(100, 100, 100, 100).ToZPLString();

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^FO100,100\n^GB100,100,1,B,0^FS", output);
        }

        [TestMethod]
        public void MultipleElements()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZPLFont(fontWidth: 50, fontHeight: 50);

            var elements = new List<ZPLElementBase>();
            elements.Add(new ZPLTextField(sampleText, 50, 100, font));
            elements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));
            elements.Add(new ZPLGraphicBox(450, 750, 100, 100, 50, ZPLConstants.LineColor.White));
            elements.Add(new ZPLGraphicCircle(400, 700, 100, 5));
            elements.Add(new ZPLGraphicDiagonalLine(400, 700, 100, 50, 5));
            elements.Add(new ZPLGraphicDiagonalLine(400, 700, 50, 100, 5));
            elements.Add(new ZPLGraphicSymbol(GraphicSymbolCharacter.Copyright, 600, 600, 50, 50));
            elements.Add(new ZPLQRCode("MM,AAC-42", 200, 800));

            //Add raw ZPL code
            elements.Add(new ZPLRaw("^FO200, 200^GB300, 200, 10 ^FS"));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void ChangeDPI()
        {
            var elements = new List<ZPLElementBase>();
            elements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));

            var options = new ZPLRenderOptions { SourcePrintDPI = 203, TargetPrintDPI = 300 };
            var output = new ZPLEngine(elements).ToZPLString(options);

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n^FO591,1034\n^GB147,147,7,B,0^FS\n^XZ", output);
        }

        [TestMethod]
        public void LayoutWithOriginOffset()
        {
            var elements = new List<ZPLElementBase>();

            var o = new ZPLOrigin(100, 100);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    elements.Add(new ZPLGraphicBox(o.PositionX, o.PositionY, 50, 50));
                    o = o.Offset(0, 100);
                }
                o = o.Offset(100, -300);
            }

            var options = new ZPLRenderOptions();
            var output = new ZPLEngine(elements).ToZPLString(options);

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void RenderComments()
        {
            var elements = new List<ZPLElementBase>();

            var textField = new ZPLTextField("AAA", 50, 100, ZPLConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { DisplayComments = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void TextFieldVariations()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZPLFont(fontWidth: 50, fontHeight: 50);

            var elements = new List<ZPLElementBase>();
            //Specail character is repalced with space
            elements.Add(new ZPLTextField(sampleText, 10, 10, font, useHexadecimalIndicator: false));
            //Specail character is using Hex value ^FH
            elements.Add(new ZPLTextField(sampleText, 10, 50, font, useHexadecimalIndicator: true));
            //Only the first line is displayed
            elements.Add(new ZPLSingleLineFieldBlock(sampleText, 10, 150, 500, font));
            //Max 2 lines, text exceeding the maximum number of lines overwrites the last line.
            elements.Add(new ZPLFieldBlock(sampleText, 10, 300, 400, font, 2));
            // Multi - line text within a box region
            elements.Add(new ZPLTextBlock(sampleText, 10, 600, 400, 100, font));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void WithoutAutoElements()
        {
            var elements = new List<ZPLElementBase>();

            var textField = new ZPLTextField("Pure element ZPL only", 50, 100, ZPLConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions { DisplayComments = true, AddDefaultLabelHome = false, AddStartEndFormat = false });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^CI28\n^FX\n//A important field\n^A0N,30,30\n^FO50,100\n^FH^FDPure element ZPL only^FS", output);
        }
    }
}
