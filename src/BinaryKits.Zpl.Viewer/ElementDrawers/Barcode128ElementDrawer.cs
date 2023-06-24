using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ZXing.OneD;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Code 128 Barcode elements
    /// </summary>
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {
        /// <summary>
        /// Start sequence lookups.
        /// <see href="https://supportcommunity.zebra.com/s/article/Creating-GS1-Barcodes-with-Zebra-Printers-for-Data-Matrix-and-Code-128-using-ZPL"/>
        /// </summary>
        private static readonly Dictionary<string, Code128EncodingOptions.Codesets> startCodeMap = new()
        {
            { ">9", Code128EncodingOptions.Codesets.A },
            { ">:", Code128EncodingOptions.Codesets.B },
            { ">;", Code128EncodingOptions.Codesets.C }
        };

        private static readonly Regex startCodeRegex = new Regex(@"^(>[9:;])(.+)$", RegexOptions.Compiled);
        private static readonly Regex invalidInvocationRegex = new Regex(@"(?<!^)>[9:;]", RegexOptions.Compiled);

        // As defined in ZXing.OneD.Code128Writer
        private static readonly string FNC1 = Convert.ToChar(241).ToString();

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode128;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcode128 barcode)
            {
                var encodingOptions = new Code128EncodingOptions();
                encodingOptions.Margin = 0;
                // remove any start sequences not at the start of the content (invalid invocation)
                string content = invalidInvocationRegex.Replace(barcode.Content, "");
                string interpretation = content;
                if (string.IsNullOrEmpty(barcode.Mode) || barcode.Mode == "N")
                {
                    Match startCodeMatch = startCodeRegex.Match(content);
                    if (startCodeMatch.Success)
                    {
                        encodingOptions.ForceCodeset = startCodeMap[startCodeMatch.Groups[1].Value];
                        content = startCodeMatch.Groups[2].Value;
                        interpretation = content;
                    }
                    else
                    {
                        encodingOptions.ForceCodeset = Code128EncodingOptions.Codesets.B;
                    }
                    // support hand-rolled GS1
                    content = content.Replace(">8", FNC1);
                    interpretation = interpretation.Replace(">8", "");
                    // TODO: support remaining escapes within a barcode
                }
                else if (barcode.Mode == "A")
                {
                    encodingOptions.CompactEncoding = true;
                }
                else if (barcode.Mode == "D")
                {
                    encodingOptions.GS1Format = true;
                    content = content.Replace(">8", FNC1);
                    interpretation = interpretation.Replace(">8", "");
                    if (!content.StartsWith(FNC1))
                    {
                        content = FNC1 + content;
                    }
                }
                else if (barcode.Mode == "U")
                {
                    encodingOptions.ForceCodeset = Code128EncodingOptions.Codesets.C;
                    content = content.PadLeft(19, '0').Substring(0, 19);
                    int checksum = 0;
                    for (int i = 0; i < 19; i++)
                    {
                        checksum += (content[i] - 48) * (i % 2 * 2 + 7);
                    }
                    interpretation = string.Format("{0}{1}", interpretation, checksum % 10);
                    content = string.Format("{0}{1}{2}", FNC1, content, checksum % 10);
                }

                float x = barcode.PositionX;
                float y = barcode.PositionY;

                var writer = new Code128Writer();
                var result = writer.encode(content, ZXing.BarcodeFormat.CODE_128, 0, 0, encodingOptions.Hints);
                using var resizedImage = this.BitArrayToSKBitmap(result.getRow(0, null), barcode.Height, barcode.ModuleWidth);
                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    // TODO: use font 0, auto scale for Mode D
                    float labelFontSize = Math.Min(barcode.ModuleWidth * 9f, 90f);
                    var labelTypeFace = options.FontLoader("A");
                    var labelFont = new SKFont(labelTypeFace, labelFontSize);
                    this.DrawInterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.PrintInterpretationLineAboveCode);
                }
            }
        }

    }
}
