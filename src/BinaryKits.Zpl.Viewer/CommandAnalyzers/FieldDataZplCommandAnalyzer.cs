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
            var x = 0;
            var y = 0;
            var bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            var zplCommandData = zplCommand.Substring(this.PrinterCommandPrefix.Length);
            var text = zplCommandData;

            if (this.VirtualPrinter.NextFieldDataElement != null)
            {
                var moduleWidth = this.VirtualPrinter.BarcodeInfo.ModuleWidth;
                var wideBarToNarrowBarWidthRatio = this.VirtualPrinter.BarcodeInfo.WideBarToNarrowBarWidthRatio;

                if (this.VirtualPrinter.NextFieldDataElement is Code39BarcodeFieldData code39)
                {
                    return new ZplBarcode39(text, x, y, code39.Height, moduleWidth, wideBarToNarrowBarWidthRatio, code39.FieldOrientation, code39.PrintInterpretationLine, code39.PrintInterpretationLineAboveCode, code39.Mod43CheckDigit, bottomToTop: bottomToTop);
                }
                if (this.VirtualPrinter.NextFieldDataElement is Code128BarcodeFieldData code128)
                {
                    return new ZplBarcode128(text, x, y, code128.Height, moduleWidth, wideBarToNarrowBarWidthRatio, code128.FieldOrientation, code128.PrintInterpretationLine, code128.PrintInterpretationLineAboveCode, bottomToTop);
                }
                if (this.VirtualPrinter.NextFieldDataElement is QrCodeBarcodeFieldData qrCode)
                {
                    return new ZplQrCode(text, x, y, 2, qrCode.MagnificationFactor, Label.ErrorCorrectionLevel.Standard, qrCode.MaskValue);
                }
            }

            var font = this.GetFontFromVirtualPrinter();
            if (this.VirtualPrinter.NextFont != null)
            {
                font = this.GetNextFontFromVirtualPrinter();
            }

            var reversePrint = this.VirtualPrinter.FieldReversePrintForNextElement;

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
