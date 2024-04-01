using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using ZXing;
using ZXing.Datamatrix.Encoder;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldDataZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex qrCodeFieldDataNormalRegex = new Regex(@"^(?<correction>[HQML])(?<input>[AM]),(?<data>.+)$", RegexOptions.Compiled);
        private static readonly Regex qrCodeFieldDataMixedRegex = new Regex(@"^D\d{4}[0-9A-F-a-f]{2},(?<correction>[HQML])(?<input>[AM]),(?<data>.+)$", RegexOptions.Compiled);
        private static readonly Regex qrCodeFieldDataModeRegex = new Regex(@"^(?:[ANK]|(?:B(?<count>\d{4})))(?<data>.+)$", RegexOptions.Compiled);

        public FieldDataZplCommandAnalyzer(VirtualPrinter virtualPrinter, string prefix="^FD") : base(prefix, virtualPrinter) { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            string text = zplCommand.Substring(this.PrinterCommandPrefix.Length);

            // If field data follows a field number, a ZplRecallFieldNumber element has to be returned
            int? fieldNumber = this.VirtualPrinter.NextFieldNumber;
            if (fieldNumber != null)
            {
                this.VirtualPrinter.ClearNextFieldNumber(); // Prevents consumption by field separator analyzer
                return new ZplRecallFieldNumber(fieldNumber.Value, text);
            }

            int x = 0;
            int y = 0;
            bool bottomToTop = false;

            if (this.VirtualPrinter.NextElementPosition != null)
            {
                x = this.VirtualPrinter.NextElementPosition.X;
                y = this.VirtualPrinter.NextElementPosition.Y;

                bottomToTop = this.VirtualPrinter.NextElementPosition.CalculateFromBottom;
            }

            if (this.VirtualPrinter.NextElementFieldData != null)
            {
                int moduleWidth = this.VirtualPrinter.BarcodeInfo.ModuleWidth;
                double wideBarToNarrowBarWidthRatio = this.VirtualPrinter.BarcodeInfo.WideBarToNarrowBarWidthRatio;

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
                    (ErrorCorrectionLevel errorCorrection, string parsedText) = ParseQrCodeFieldData(qrCode, text);

                    // N.B.: always pass Field Orientation Normal to QR codes; the ZPL II standard does not allow rotation
                    return new ZplQrCode(parsedText, x, y, qrCode.Model, qrCode.MagnificationFactor, errorCorrection, qrCode.MaskValue, Label.FieldOrientation.Normal, bottomToTop);
                }
                if (this.VirtualPrinter.NextElementFieldData is PDF417FieldData pdf147)
                {
                    return new ZplPDF417(text, x, y, pdf147.Height, moduleWidth, pdf147.Columns, pdf147.Rows, pdf147.Compact, pdf147.SecurityLevel, pdf147.FieldOrientation, bottomToTop);
                }
            }

            var font = this.GetFontFromVirtualPrinter();
            if (this.VirtualPrinter.NextFont != null)
            {
                font = this.GetNextFontFromVirtualPrinter();
            }

            bool reversePrint = this.VirtualPrinter.NextElementFieldReverse || this.VirtualPrinter.LabelReverse;

            if (this.VirtualPrinter.NextElementFieldBlock != null)
            {
                int width = this.VirtualPrinter.NextElementFieldBlock.WidthOfTextBlockLine;
                int maxLineCount = this.VirtualPrinter.NextElementFieldBlock.MaximumNumberOfLinesInTextBlock;
                var textJustification = this.VirtualPrinter.NextElementFieldBlock.TextJustification;
                int lineSpace = this.VirtualPrinter.NextElementFieldBlock.AddOrDeleteSpaceBetweenLines;
                int hangingIndent = this.VirtualPrinter.NextElementFieldBlock.HangingIndentOfTheSecondAndRemainingLines;

                return new ZplFieldBlock(text, x, y, width, font, maxLineCount, lineSpace, textJustification, hangingIndent, reversePrint : reversePrint, bottomToTop: bottomToTop);
            }

            return new ZplTextField(text, x, y, font, reversePrint: reversePrint, bottomToTop: bottomToTop);
        }

        private (ErrorCorrectionLevel, string) ParseQrCodeFieldData(QrCodeBarcodeFieldData qrCode, string text)
        {
            ErrorCorrectionLevel errorCorrection = qrCode.ErrorCorrection;
            string parsedText = text;

            Match normalMatch = qrCodeFieldDataNormalRegex.Match(text);
            if (normalMatch.Success)
            {
                errorCorrection = this.ConvertErrorCorrectionLevel(normalMatch.Groups["correction"].Value);
                string input = normalMatch.Groups["input"].Value;
                string fullData = normalMatch.Groups["data"].Value;
                if (input == "A")
                {
                    parsedText = fullData;
                }
                else if (input == "M")
                {
                    Match modeMatch = qrCodeFieldDataModeRegex.Match(fullData);
                    if (modeMatch.Success)
                    {
                        if (modeMatch.Groups["count"].Success)
                        {
                            int count = Math.Min(int.Parse(modeMatch.Groups["count"].Value), fullData.Length);
                            parsedText = modeMatch.Groups["data"].Value.Substring(0, count);
                        }
                        else
                        {
                            parsedText = modeMatch.Groups["data"].Value;
                        }
                    }
                    else
                    {
                        parsedText = fullData;
                    }
                }
            }
            else
            {
                Match mixedMatch = qrCodeFieldDataMixedRegex.Match(text);
                if (mixedMatch.Success)
                {
                    errorCorrection = this.ConvertErrorCorrectionLevel(mixedMatch.Groups["correction"].Value);
                    string input = mixedMatch.Groups["input"].Value;
                    string fullData = mixedMatch.Groups["data"].Value;
                    if (input == "A")
                    {
                        string[] dataParts = fullData.Split(',');
                        parsedText = string.Join("", dataParts);
                    }
                    else if (input == "M")
                    {
                        StringBuilder builder = new StringBuilder();
                        while (fullData.Length > 0)
                        {
                            Match modeMatch = qrCodeFieldDataModeRegex.Match(fullData);
                            if (modeMatch.Success)
                            {
                                string data = modeMatch.Groups["data"].Value;
                                if (modeMatch.Groups["count"].Success)
                                {
                                    int count = Math.Min(int.Parse(modeMatch.Groups["count"].Value), data.Length);
                                    builder.Append(data.Substring(0, count));
                                    fullData = data.Substring(count);
                                }
                                else
                                {
                                    string[] dataParts = data.Split(new char[] { ',' }, 2);
                                    builder.Append(dataParts[0]);
                                    fullData = dataParts.Length > 1 ? dataParts[1] : string.Empty;
                                }
                            }
                            else
                            {
                                string[] dataParts = fullData.Split(new char[] { ',' }, 2);
                                builder.Append(dataParts[0]);
                                fullData = dataParts.Length > 1 ? dataParts[1] : string.Empty;
                            }
                        }

                        parsedText = builder.ToString();
                    }
                }

            }

            return (errorCorrection, parsedText);
        }

        private ZplFont GetFontFromVirtualPrinter()
        {
            int fontWidth = this.VirtualPrinter.FontWidth;
            int fontHeight = this.VirtualPrinter.FontHeight;
            string fontName = this.VirtualPrinter.FontName;

            return new ZplFont(fontWidth, fontHeight, fontName);
        }

        private ZplFont GetNextFontFromVirtualPrinter()
        {
            int fontWidth = this.VirtualPrinter.NextFont.FontWidth;
            int fontHeight = this.VirtualPrinter.NextFont.FontHeight;
            string fontName = this.VirtualPrinter.NextFont.FontName;
            var fieldOrientation = this.VirtualPrinter.NextFont.FieldOrientation;

            return new ZplFont(fontWidth, fontHeight, fontName, fieldOrientation);
        }
    }
}
