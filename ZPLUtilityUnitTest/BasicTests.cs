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
            var elements = new List<ZPLElementBase>();
            elements.Add(new ZPLTextField(sampleText, 50, 100, font));
            elements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));
            elements.Add(new ZPLGraphicBox(450, 750, 100, 100, 50, ZPLConstants.LineColor.White));
            elements.Add(new ZPLGraphicCircle(400, 700, 100, 5));
            elements.Add(new ZPLGraphicDiagonalLine(400, 700, 100, 50, 5));
            elements.Add(new ZPLGraphicDiagonalLine(400, 700, 50, 100, 5));
            elements.Add(new ZPLGraphicSymbol(ZPLGraphicSymbol.GraphicSymbolCharacter.Copyright, 600, 600, 50, 50));
            elements.Add(new ZPLQRCode("MM,AAC-42", 200, 800));

            //Add raw ZPL code
            elements.Add(new ZPLRaw("^FO200, 200^GB300, 200, 10 ^FS"));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void Barcode()
        {
            var elements = new List<ZPLElementBase>();

            elements.Add(new ZPLBarCode39("123ABC", 100, 100));
            elements.Add(new ZPLBarCode128("123ABC", 100, 300));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void ChangeDPI()
        {
            var elements = new List<ZPLElementBase>();
            elements.Add(new ZPLGraphicBox(400, 700, 100, 100, 5));

            var options = new ZPLRenderOptions() { SourcePrintDPI = 203, TargetPrintDPI = 300 };
            var output = new ZPLEngine(elements).ToZPLString(options);

            Console.WriteLine(output);
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

            Console.WriteLine(output);
        }

        [TestMethod]
        public void RenderComments()
        {
            var elements = new List<ZPLElementBase>();

            var textField = new ZPLTextField("AAA", 50, 100, ZPLConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { DisplayComments = true });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void TextFieldVariations()
        {
            var sampleText = "[_~^][LineBreak\n][The quick fox jumps over the lazy dog.]";
            ZPLFont font = new ZPLFont(fontWidth: 50, fontHeight: 50);

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
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void DownloadGraphics()
        {
            var elements = new List<ZPLElementBase>();
            elements.Add(new ZPLGraphicBox(0, 0, 100, 100, 4));

            elements.Add(new ZPLDownloadGraphics('R', "SAMPLE", "GRC", new System.Drawing.Bitmap("p.jpg")));
            elements.Add(new ZPLRecallGraphic(100, 100, 'R', "SAMPLE", "GRC"));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true, TargetPrintDPI = 200, SourcePrintDPI = 200 });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void DownloadObjets()
        {
            var elements = new List<ZPLElementBase>();

            elements.Add(new ZPLGraphicBox(0, 0, 100, 100, 4));
            elements.Add(new ZPLDownloadObjects('R', "SAMPLE.PNG", new System.Drawing.Bitmap("sample.bmp")));
            elements.Add(new ZPLImageMove(100, 100, 'R', "SAMPLE", "PNG"));

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { AddEmptyLineBeforeElementStart = true, TargetPrintDPI = 300, SourcePrintDPI = 200 });

            Console.WriteLine(output);
        }

        [TestMethod]
        public void WithoutAutoElements()
        {
            var elements = new List<ZPLElementBase>();

            var textField = new ZPLTextField("Pure element ZPL only", 50, 100, ZPLConstants.Font.Default);
            textField.Comments.Add("A important field");
            elements.Add(textField);

            var renderEngine = new ZPLEngine(elements);
            var output = renderEngine.ToZPLString(new ZPLRenderOptions() { DisplayComments = true, AddDefaultLabelHome = false, AddStartEndFormat = false });

            Console.WriteLine(output);
        }
    }
}
