using BinaryKits.Zpl.Label.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void SingelElement()
        {
            var output = new ZplGraphicBox(100, 100, 100, 100).ToZplString();

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^FO100,100\n^GB100,100,1,B,0^FS", output);
        }

        [TestMethod]
        public void MultipleElements()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZplFont(fontWidth: 50, fontHeight: 50);

            var elements = new List<ZplElementBase>();
            elements.Add(new ZplTextField(sampleText, 50, 100, font));
            elements.Add(new ZplGraphicBox(400, 700, 100, 100, 5));
            elements.Add(new ZplGraphicBox(450, 750, 100, 100, 50, LineColor.White));
            elements.Add(new ZplGraphicCircle(400, 700, 100, 5));
            elements.Add(new ZplGraphicDiagonalLine(400, 700, 100, 50, 5));
            elements.Add(new ZplGraphicDiagonalLine(400, 700, 50, 100, 5));
            elements.Add(new ZplGraphicSymbol(GraphicSymbolCharacter.Copyright, 600, 600, 50, 50));
            elements.Add(new ZplQrCode("MM,AAC-42", 200, 800));

            //Add raw Zpl code
            elements.Add(new ZplRaw("^FO200, 200^GB300, 200, 10 ^FS"));

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void ChangeDPI()
        {
            var elements = new List<ZplElementBase>();
            elements.Add(new ZplGraphicBox(400, 700, 100, 100, 5));

            var options = new ZplRenderOptions { SourcePrintDpi = 203, TargetPrintDpi = 300 };
            var output = new ZplEngine(elements).ToZplString(options);

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^XA\n^LH0,0\n^CI28\n^FO591,1034\n^GB147,147,7,B,0^FS\n^XZ", output);
        }

        [TestMethod]
        public void LayoutWithOriginOffset()
        {
            var elements = new List<ZplElementBase>();

            var origin = new ZplFieldOrigin(100, 100);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    elements.Add(new ZplGraphicBox(origin.PositionX, origin.PositionY, 50, 50));
                    origin = origin.Offset(0, 100);
                }
                origin = origin.Offset(100, -300);
            }

            var options = new ZplRenderOptions();
            var output = new ZplEngine(elements).ToZplString(options);

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void RenderComments()
        {
            var elements = new List<ZplElementBase>();

            var textField = new ZplTextField("AAA", 50, 100, ZplConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { DisplayComments = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void TextFieldVariations()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            var font = new ZplFont(fontWidth: 50, fontHeight: 50);

            var elements = new List<ZplElementBase>();
            //Specail character is repalced with space
            elements.Add(new ZplTextField(sampleText, 10, 10, font, useHexadecimalIndicator: false));
            //Specail character is using Hex value ^FH
            elements.Add(new ZplTextField(sampleText, 10, 50, font, useHexadecimalIndicator: true));
            //Only the first line is displayed
            elements.Add(new ZplSingleLineFieldBlock(sampleText, 10, 150, 500, font));
            //Max 2 lines, text exceeding the maximum number of lines overwrites the last line.
            elements.Add(new ZplFieldBlock(sampleText, 10, 300, 400, font, 2));
            // Multi - line text within a box region
            elements.Add(new ZplTextBlock(sampleText, 10, 600, 400, 100, font));

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { AddEmptyLineBeforeElementStart = true });

            Debug.WriteLine(output);
        }

        [TestMethod]
        public void WithoutAutoElements()
        {
            var elements = new List<ZplElementBase>();

            var textField = new ZplTextField("Pure element zpl only", 50, 100, ZplConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZplEngine(elements);
            var output = renderEngine.ToZplString(new ZplRenderOptions { DisplayComments = true, AddDefaultLabelHome = false, AddStartEndFormat = false });

            Debug.WriteLine(output);
            Assert.IsNotNull(output);
            Assert.AreEqual("^CI28\n^FX\n//A important field\n^A0N,30,30\n^FO50,100\n^FH^FDPure element zpl only^FS", output);
        }
    }
}
