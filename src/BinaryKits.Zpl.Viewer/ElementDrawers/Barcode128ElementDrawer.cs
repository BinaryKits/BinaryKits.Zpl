using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {

        /// <summary>
        /// Start sequence lookups.
        /// <see cref="https://supportcommunity.zebra.com/s/article/Creating-GS1-Barcodes-with-Zebra-Printers-for-Data-Matrix-and-Code-128-using-ZPL"/>
        /// </summary>
        private static readonly Dictionary<string, TYPE> startCodeMap = new Dictionary<string, TYPE>()
        {
            { ">9", TYPE.CODE128A },
            { ">:", TYPE.CODE128B },
            { ">;", TYPE.CODE128C }
        };

        private static readonly Regex startCodeRegex = new Regex(@"^(>[9:;])(.+)$", RegexOptions.Compiled);

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
                string content = barcode.Content;
                if(string.IsNullOrEmpty(barcode.Mode) || barcode.Mode == "N")
                {
                    Match startCodeMatch = startCodeRegex.Match(content);
                    if(startCodeMatch.Success)
                    {
                        barcodeType = startCodeMap[startCodeMatch.Groups[1].Value];
                        content = startCodeMatch.Groups[2].Value;
                    }
                    // TODO: support escapes within a barcode, not only start sequences
                }
                else if(barcode.Mode == "A")
                {
                    barcodeType = TYPE.CODE128; // dynamic
                }
                else
                {
                    // TODO: support for mode D/U
                }

                float x = barcode.PositionX;
                float y = barcode.PositionY;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                float labelFontSize = barcode.ModuleWidth * 7.25f;
                var labelTypeFace = options.FontLoader("1");
                var labelFont = new SKFont(labelTypeFace, labelFontSize).ToSystemDrawingFont();
                int labelHeight = barcode.PrintInterpretationLine ? labelFont.Height : 0;

                var barcodeElement = new Barcode
                {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height + labelHeight,
                    IncludeLabel = barcode.PrintInterpretationLine,
                    LabelPosition = barcode.PrintInterpretationLineAboveCode ? LabelPositions.TOPCENTER : LabelPositions.BOTTOMCENTER,
                    LabelFont = labelFont
                };

                if(barcode.PrintInterpretationLineAboveCode)
                {
                    y -= labelHeight;
                }

                Image image = barcodeElement.Encode(barcodeType, content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height + labelHeight, image.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
