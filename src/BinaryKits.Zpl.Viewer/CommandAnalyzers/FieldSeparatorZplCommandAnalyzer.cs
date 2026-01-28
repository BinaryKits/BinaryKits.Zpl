using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldSeparatorZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly Regex qrCodeFieldDataNormalRegex = new(@"^(?<correction>[HQML])(?<input>[AM]),(?<data>.+)$", RegexOptions.Compiled);
        private static readonly Regex qrCodeFieldDataMixedRegex = new(@"^D\d{4}[0-9A-F-a-f]{2},(?<correction>[HQML])(?<input>[AM]),(?<data>.+)$", RegexOptions.Compiled);
        private static readonly Regex qrCodeFieldDataModeRegex = new(@"^(?:[ANK]|(?:B(?<count>\d{4})))(?<data>.+)$", RegexOptions.Compiled);

        private static readonly FieldDataZplCommandAnalyzer fieldDataAnalyzer = new();

        public FieldSeparatorZplCommandAnalyzer() : base("^FS") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            // If next field number has been set and was not consumed by a field data
            // it has to be stored as a command so that it is handled when merging formats
            ZplElementBase element = null;
            int? fieldNumber = virtualPrinter.NextFieldNumber;
            if (fieldNumber.HasValue)
            {
                virtualPrinter.ClearNextFieldNumber();
                fieldDataAnalyzer.Analyze(zplCommand, virtualPrinter, printerStorage);
                ZplElementBase dataElement = this.FinalizeField(zplCommand, virtualPrinter, printerStorage);
                element = new ZplFieldNumber(fieldNumber.Value, dataElement);
            }
            else
            {
                element = this.FinalizeField(zplCommand, virtualPrinter, printerStorage);
            }

            virtualPrinter.ClearNextElementPosition();
            virtualPrinter.ClearNextElementFieldBlock();
            virtualPrinter.ClearNextElementFieldData();
            virtualPrinter.ClearNextElementFieldReverse();
            virtualPrinter.ClearNextElementFieldHexadecimalIndicator();
            virtualPrinter.ClearNextElementFieldJustification();
            virtualPrinter.ClearNextFont();
            virtualPrinter.ClearComments();

            return element;
        }

        private ZplElementBase FinalizeField(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            int x = 0;
            int y = 0;
            char? hexadecimalIndicator = virtualPrinter.NextElementFieldHexadecimalIndicator;
            bool bottomToTop = false;
            bool useDefaultPosition = false;
            string content = virtualPrinter.NextElementFieldData?.Content;

            FieldJustification fieldJustification = virtualPrinter.NextElementFieldJustification;
            if (fieldJustification == FieldJustification.None)
            {
                fieldJustification = virtualPrinter.FieldJustification;
            }

            if (virtualPrinter.NextElementPosition != null)
            {
                x = virtualPrinter.NextElementPosition.X;
                y = virtualPrinter.NextElementPosition.Y;

                bottomToTop = virtualPrinter.NextElementPosition.CalculateFromBottom;
                useDefaultPosition = virtualPrinter.NextElementPosition.UseDefaultPosition;
            }

            if (virtualPrinter.NextElementFieldData != null)
            {
                int moduleWidth = virtualPrinter.BarcodeInfo.ModuleWidth;
                int barcodeHeight = virtualPrinter.BarcodeInfo.Height;
                double wideBarToNarrowBarWidthRatio = virtualPrinter.BarcodeInfo.WideBarToNarrowBarWidthRatio;

                if (virtualPrinter.NextElementFieldData is Code39BarcodeFieldData code39)
                {
                    return new ZplBarcode39(content, x, y, code39.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, code39.FieldOrientation, hexadecimalIndicator, code39.PrintInterpretationLine, code39.PrintInterpretationLineAboveCode, code39.Mod43CheckDigit, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is Code93BarcodeFieldData code93)
                {
                    return new ZplBarcode93(content, x, y, code93.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, code93.FieldOrientation, hexadecimalIndicator, code93.PrintInterpretationLine, code93.PrintInterpretationLineAboveCode, code93.PrintCheckDigit, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is Code128BarcodeFieldData code128)
                {
                    return new ZplBarcode128(content, x, y, code128.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, code128.FieldOrientation, hexadecimalIndicator, code128.PrintInterpretationLine, code128.PrintInterpretationLineAboveCode, bottomToTop, useDefaultPosition, code128.Mode);
                }
                else if (virtualPrinter.NextElementFieldData is CodeEAN13BarcodeFieldData codeEAN13)
                {
                    return new ZplBarcodeEan13(content, x, y, codeEAN13.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, codeEAN13.FieldOrientation, hexadecimalIndicator, codeEAN13.PrintInterpretationLine, codeEAN13.PrintInterpretationLineAboveCode, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is DataMatrixFieldData dataMatrixFieldData)
                {
                    return new ZplDataMatrix(content, x, y, dataMatrixFieldData.Height ?? barcodeHeight, dataMatrixFieldData.QualityLevel, dataMatrixFieldData.FieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is Interleaved2of5BarcodeFieldData interleaved2of5)
                {
                    return new ZplBarcodeInterleaved2of5(content, x, y, interleaved2of5.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, interleaved2of5.FieldOrientation, hexadecimalIndicator, interleaved2of5.PrintInterpretationLine, interleaved2of5.PrintInterpretationLineAboveCode, bottomToTop: bottomToTop, useDefaultPosition: useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is MaxiCodeBarcodeFieldData maxiCode)
                {
                    return new ZplMaxiCode(content, x, y, maxiCode.Mode, maxiCode.Position, maxiCode.Total, hexadecimalIndicator, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is QrCodeBarcodeFieldData qrCode)
                {
                    (ErrorCorrectionLevel errorCorrection, string parsedText) = this.ParseQrCodeFieldData(qrCode, content);
                    // N.B.: always pass Field Orientation Normal to QR codes; the ZPL II standard does not allow rotation
                    return new ZplQrCode(parsedText, x, y, qrCode.Model, qrCode.MagnificationFactor, errorCorrection, qrCode.MaskValue, FieldOrientation.Normal, hexadecimalIndicator, bottomToTop, useDefaultPosition, barcodeHeight);
                }
                else if (virtualPrinter.NextElementFieldData is UpcABarcodeFieldData upcA)
                {
                    return new ZplBarcodeUpcA(content, x, y, upcA.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, upcA.FieldOrientation, hexadecimalIndicator, upcA.PrintInterpretationLine, upcA.PrintInterpretationLineAboveCode, upcA.PrintCheckDigit, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is UpcEBarcodeFieldData upcE)
                {
                    return new ZplBarcodeUpcE(content, x, y, upcE.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, upcE.FieldOrientation, hexadecimalIndicator, upcE.PrintInterpretationLine, upcE.PrintInterpretationLineAboveCode, upcE.PrintCheckDigit, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is UpcExtensionBarcodeFieldData upcExt)
                {
                    return    new ZplBarcodeUpcExtension(content, x, y, upcExt.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, upcExt.FieldOrientation, hexadecimalIndicator, upcExt.PrintInterpretationLine, upcExt.PrintInterpretationLineAboveCode, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is PDF417FieldData pdf147)
                {
                    return new ZplPDF417(content, x, y, pdf147.Height ?? barcodeHeight, moduleWidth, pdf147.Columns, pdf147.Rows, pdf147.Compact, pdf147.SecurityLevel, pdf147.FieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is AztecBarcodeFieldData aztec)
                {
                    return new ZplAztecBarcode(content, x, y, aztec.MagnificationFactor, aztec.ExtendedChannel, aztec.ErrorControl, aztec.MenuSymbol, aztec.SymbolCount, aztec.IdField, aztec.FieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is AnsiCodabarFieldData codabar)
                {
                    return new ZplBarcodeAnsiCodabar(content, codabar.StartCharacter, codabar.StopCharacter, x, y, codabar.Height ?? barcodeHeight, moduleWidth, wideBarToNarrowBarWidthRatio, codabar.FieldOrientation, hexadecimalIndicator, codabar.PrintInterpretationLine, codabar.PrintInterpretationLineAboveCode, codabar.CheckDigit, bottomToTop, useDefaultPosition);
                }
                else if (virtualPrinter.NextElementFieldData is GraphicSymbolFieldData graphicSymbol)
                {
                    return new ZplGraphicSymbol(content, x, y, graphicSymbol.Width, graphicSymbol.Height, graphicSymbol.FieldOrientation, bottomToTop, useDefaultPosition);
                }
                else
                {
                    ZplFont font = this.GetFontFromVirtualPrinter(virtualPrinter);
                    if (virtualPrinter.NextFont != null)
                    {
                        font = this.GetNextFontFromVirtualPrinter(virtualPrinter);
                    }

                    bool reversePrint = virtualPrinter.NextElementFieldReverse || virtualPrinter.LabelReverse;

                    if (virtualPrinter.NextElementFieldBlock != null)
                    {
                        int width = virtualPrinter.NextElementFieldBlock.WidthOfTextBlockLine;
                        int maxLineCount = virtualPrinter.NextElementFieldBlock.MaximumNumberOfLinesInTextBlock;
                        TextJustification textJustification = virtualPrinter.NextElementFieldBlock.TextJustification;
                        int lineSpace = virtualPrinter.NextElementFieldBlock.AddOrDeleteSpaceBetweenLines;
                        int hangingIndent = virtualPrinter.NextElementFieldBlock.HangingIndentOfTheSecondAndRemainingLines;

                        return new ZplFieldBlock(content, x, y, width, font, maxLineCount, lineSpace, textJustification, hangingIndent, hexadecimalIndicator: hexadecimalIndicator, reversePrint: reversePrint, bottomToTop: bottomToTop, useDefaultPosition: useDefaultPosition);
                    }

                    return new ZplTextField(content, x, y, font, hexadecimalIndicator: hexadecimalIndicator, reversePrint: reversePrint, bottomToTop: bottomToTop, fieldJustification: fieldJustification, useDefaultPosition: useDefaultPosition);
                }
            }

            return null;
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
                        StringBuilder builder = new();
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
                                    string[] dataParts = data.Split([','], 2);
                                    builder.Append(dataParts[0]);
                                    fullData = dataParts.Length > 1 ? dataParts[1] : string.Empty;
                                }
                            }
                            else
                            {
                                string[] dataParts = fullData.Split([','], 2);
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

        private ZplFont GetFontFromVirtualPrinter(VirtualPrinter virtualPrinter)
        {
            int fontWidth = virtualPrinter.FontWidth;
            int fontHeight = virtualPrinter.FontHeight;
            string fontName = virtualPrinter.FontName;
            FieldOrientation fieldOrientation = virtualPrinter.FieldOrientation;

            return new ZplFont(fontWidth, fontHeight, fontName, fieldOrientation);
        }

        private ZplFont GetNextFontFromVirtualPrinter(VirtualPrinter virtualPrinter)
        {
            int fontWidth = virtualPrinter.NextFont.FontWidth;
            int fontHeight = virtualPrinter.NextFont.FontHeight;
            string fontName = virtualPrinter.NextFont.FontName;
            FieldOrientation fieldOrientation = virtualPrinter.NextFont.FieldOrientation;

            return new ZplFont(fontWidth, fontHeight, fontName, fieldOrientation);
        }
    }
}
