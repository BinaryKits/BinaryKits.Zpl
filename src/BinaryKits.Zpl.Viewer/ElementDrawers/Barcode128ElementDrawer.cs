using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {

        /// <summary>
        /// Start sequence lookups.
        /// <see href="https://supportcommunity.zebra.com/s/article/Creating-GS1-Barcodes-with-Zebra-Printers-for-Data-Matrix-and-Code-128-using-ZPL"/>
        /// </summary>
        private static readonly Dictionary<string, TYPE> startCodeMap = new Dictionary<string, TYPE>()
        {
            { ">9", TYPE.CODE128A },
            { ">:", TYPE.CODE128B },
            { ">;", TYPE.CODE128C }
        };

        private static readonly Regex startCodeRegex = new Regex(@"^(>[9:;])(.+)$", RegexOptions.Compiled);
        private static readonly Regex invalidInvocationRegex = new Regex(@"(?<!^)>[9:;]", RegexOptions.Compiled);

        // As defined in BarcodeLib.Symbologies.Code128
        private static readonly string FNC1 = Convert.ToChar(200).ToString();

        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode128;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcode128 barcode)
            {
                var barcodeType = TYPE.CODE128B;
                // remove any start sequences not at the start of the content (invalid invocation)
                string content = invalidInvocationRegex.Replace(barcode.Content, "");
                string interpretation = content;
                if (string.IsNullOrEmpty(barcode.Mode) || barcode.Mode == "N")
                {
                    Match startCodeMatch = startCodeRegex.Match(content);
                    if (startCodeMatch.Success)
                    {
                        barcodeType = startCodeMap[startCodeMatch.Groups[1].Value];
                        content = startCodeMatch.Groups[2].Value;
                        interpretation = content;
                    }
                    // support hand-rolled GS1
                    content = content.Replace(">8", FNC1);
                    interpretation = interpretation.Replace(">8", "");
                    // TODO: support remaining escapes within a barcode
                }
                else if (barcode.Mode == "A")
                {
                    barcodeType = TYPE.CODE128; // dynamic
                }
                else if (barcode.Mode == "D")
                {
                    barcodeType = TYPE.CODE128C;
                    content = content.Replace(">8", FNC1);
                    interpretation = interpretation.Replace(">8", "");
                    if (!content.StartsWith(FNC1))
                    {
                        content = FNC1 + content;
                    }
                }
                else if (barcode.Mode == "U")
                {
                    barcodeType = TYPE.CODE128C;
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

                float labelFontSize = Math.Min(barcode.ModuleWidth * 7.2f, 72f);
                var labelTypeFace = options.FontLoader("A");
                var labelFont = new SKFont(labelTypeFace, labelFontSize).ToSystemDrawingFont();
                int labelHeight = barcode.PrintInterpretationLine ? labelFont.Height : 0;
                int labelHeightOffset = barcode.PrintInterpretationLineAboveCode ? labelHeight : 0;

                var barcodeElement = new Barcode
                {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height + labelHeight,
                    IncludeLabel = barcode.PrintInterpretationLine,
                    LabelPosition = barcode.PrintInterpretationLineAboveCode ? LabelPositions.TOPCENTER : LabelPositions.BOTTOMCENTER,
                    LabelFont = labelFont,
                    AlternateLabel = interpretation
                };

                using var image = barcodeElement.Encode(barcodeType, content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height, image.Width, barcode.FieldOrigin != null, x, y, labelHeightOffset, barcode.FieldOrientation);
            }
        }
    }
}
