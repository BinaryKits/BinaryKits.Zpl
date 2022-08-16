using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldDataZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldDataZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FD", virtualPrinter)
        { }

        public override ZplElementBase Analyze(string zplCommand)
        {
            var text = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            // If field data follows a field number, a ZplRecallFieldNumber element has to be returned
            int? fieldNumber = this.VirtualPrinter.NextFieldNumber;
            if (fieldNumber != null)
            {
                this.VirtualPrinter.ClearNextFieldNumber(); // Prevents consumption by field separator analyzer
                return new ZplRecallFieldNumber(fieldNumber.Value, text);
            }

            var x = 0;
            var y = 0;
            var bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            if (this.VirtualPrinter.NextElementFieldData != null)
            {
                var moduleWidth = this.VirtualPrinter.BarcodeInfo.ModuleWidth;
                var wideBarToNarrowBarWidthRatio = this.VirtualPrinter.BarcodeInfo.WideBarToNarrowBarWidthRatio;

                if (this.VirtualPrinter.NextElementFieldData is Code39BarcodeFieldData code39)
                {
                    return new ZplBarcode39(text, x, y, code39.Height, moduleWidth, wideBarToNarrowBarWidthRatio, code39.FieldOrientation, code39.PrintInterpretationLine, code39.PrintInterpretationLineAboveCode, code39.Mod43CheckDigit, bottomToTop: bottomToTop);
                }
                if (this.VirtualPrinter.NextElementFieldData is Code128BarcodeFieldData code128)
                {
                    return new ZplBarcode128(text, x, y, code128.Height, moduleWidth, wideBarToNarrowBarWidthRatio, code128.FieldOrientation, code128.PrintInterpretationLine, code128.PrintInterpretationLineAboveCode, bottomToTop, code128.Mode);
                }

                if (this.VirtualPrinter.NextElementFieldData is CodeEAN13BarcodeFieldData codeEAN13)
                {
                    return new ZplBarcodeEan13(text, x, y, codeEAN13.Height, moduleWidth, wideBarToNarrowBarWidthRatio, codeEAN13.FieldOrientation, codeEAN13.PrintInterpretationLine, codeEAN13.PrintInterpretationLineAboveCode, bottomToTop);
                }
                if (this.VirtualPrinter.NextElementFieldData is DataMatrixFieldData dataMatrixFieldData)
                {
                    return new ZplDataMatrix(text, x, y, dataMatrixFieldData.Height, dataMatrixFieldData.FieldOrientation, bottomToTop);
                }
                if (this.VirtualPrinter.NextElementFieldData is Interleaved2of5BarcodeFieldData interleaved2of5)
                {
                    return new ZplBarcodeInterleaved2of5(text, x, y, interleaved2of5.Height, moduleWidth, wideBarToNarrowBarWidthRatio, interleaved2of5.FieldOrientation, interleaved2of5.PrintInterpretationLine, interleaved2of5.PrintInterpretationLineAboveCode, bottomToTop: bottomToTop);
                }
                if (this.VirtualPrinter.NextElementFieldData is QrCodeBarcodeFieldData qrCode)
                {
                    var indexOfComma = text.IndexOf(',');
                    if (indexOfComma != -1)
                    {
                        text = text.Substring(indexOfComma + 1);
                    }

                    return new ZplQrCode(text, x, y, qrCode.Model, qrCode.MagnificationFactor, qrCode.ErrorCorrection, qrCode.MaskValue);
                }
            }

            var font = this.GetFontFromVirtualPrinter();
            if (this.VirtualPrinter.NextFont != null)
            {
                font = this.GetNextFontFromVirtualPrinter();
            }

            var reversePrint = this.VirtualPrinter.NextElementFieldReverse || this.VirtualPrinter.LabelReverse;

            if (this.VirtualPrinter.NextElementFieldBlock != null)
            {
                var width = this.VirtualPrinter.NextElementFieldBlock.WidthOfTextBlockLine;
                var maxLineCount = this.VirtualPrinter.NextElementFieldBlock.MaximumNumberOfLinesInTextBlock;
                var textJustification = this.VirtualPrinter.NextElementFieldBlock.TextJustification;
                var lineSpace = this.VirtualPrinter.NextElementFieldBlock.AddOrDeleteSpaceBetweenLines;
                var hangingIndent = this.VirtualPrinter.NextElementFieldBlock.HangingIndentOfTheSecondAndRemainingLines;

                return new ZplFieldBlock(text, x, y, width, font, maxLineCount, lineSpace, textJustification, hangingIndent, reversePrint : reversePrint, bottomToTop: bottomToTop);
            }

            return new ZplTextField(text, x, y, font, reversePrint: reversePrint, bottomToTop: bottomToTop);
        }

        private ZplFont GetFontFromVirtualPrinter()
        {
            var fontWidth = this.VirtualPrinter.FontWidth;
            var fontHeight = this.VirtualPrinter.FontHeight;
            var fontName = this.VirtualPrinter.FontName;

            return new ZplFont(fontWidth, fontHeight, fontName);
        }

        private ZplFont GetNextFontFromVirtualPrinter()
        {
            var fontWidth = this.VirtualPrinter.NextFont.FontWidth;
            var fontHeight = this.VirtualPrinter.NextFont.FontHeight;
            var fontName = this.VirtualPrinter.NextFont.FontName;
            var fieldOrientation = this.VirtualPrinter.NextFont.FieldOrientation;

            return new ZplFont(fontWidth, fontHeight, fontName, fieldOrientation);
        }
    }
}
