using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldDataZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldDataZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^FD", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = this.VirtualPrinter.NextElementPosition.X;
            var y = this.VirtualPrinter.NextElementPosition.Y;

            this.VirtualPrinter.ClearNextElementPosition();

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);
            var text = zplCommandData;

            if (this.VirtualPrinter.NextFieldDataElement != null)
            {
                try
                {
                    if (this.VirtualPrinter.NextFieldDataElement is Code39BarcodeFieldData code39)
                    {
                        return new ZplBarcode39(text, x, y, code39.Height, code39.FieldOrientation, code39.PrintInterpretationLine, code39.PrintInterpretationLineAboveCode, code39.Mod43CheckDigit);
                    }
                    if (this.VirtualPrinter.NextFieldDataElement is Code128BarcodeFieldData code128)
                    {
                        return new ZplBarcode128(text, x, y, code128.Height, code128.FieldOrientation, code128.PrintInterpretationLine, code128.PrintInterpretationLineAboveCode);
                    }
                    if (this.VirtualPrinter.NextFieldDataElement is QrCodeBarcodeFieldData qrCode)
                    {
                        return new ZplQrCode(text, x, y, 2, qrCode.MagnificationFactor, Label.ErrorCorrectionLevel.Standard, qrCode.MaskValue);
                    }
                }
                finally
                {
                    this.VirtualPrinter.ClearNextFieldDataElement();
                }
            }

            var font = this.GetFontFromVirtualPrinter();
            if (this.VirtualPrinter.NextFont != null)
            {
                font = this.GetNextFontFromVirtualPrinter();
            }

            return new ZplTextField(text, x, y, font);
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

            this.VirtualPrinter.ClearNextFont();

            return new ZplFont(fontWidth, fontHeight, fontName, fieldOrientation);
        }
    }
}
