using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;
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
        private static readonly Regex startCodeRegex = new Regex(@"(>[9:;])", RegexOptions.Compiled);
        private static readonly Regex invalidInvocationRegex = new Regex(@"(?<!^)>[<0=12345679:;]", RegexOptions.Compiled); //>8 has limited support here

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
                var barcodeType = TYPE.CODE128;
                
                //remove the start code form the content we only support the globals N,A,D,U and our barcode library doesn't support these types
                string content = startCodeRegex.Replace(barcode.Content, "");
                string interpretation = content;
                
                // remove any start sequences not at the start of the content (invalid invocation)
                content = invalidInvocationRegex.Replace(content, "");
                interpretation = content;
                
                // support hand-rolled GS1
                content = content.Replace(">8", FNC1);
                interpretation = interpretation.Replace(">8", "");
                
                if (string.IsNullOrEmpty(barcode.Mode) || barcode.Mode == "N")
                {
                    barcodeType = TYPE.CODE128; // dynamic
                    //TODO: Instead of using the auto type, switch type for each part of the content
                    // - Current library doesn't support that.
                    //>:+B210AC>50270>6/$+2>5023080000582>6L
                    //[TYPE.CODE128B]+B210AC
                    //[TYPE.CODE128C]0270
                    //[TYPE.CODE128B]+/$+2
                    //[TYPE.CODE128C]023080000582
                    //[TYPE.CODE128B]L
                }
                else if (barcode.Mode == "A")
                {
                    //A (automatic mode, the ZPL engine automatically determines the subsets that are used to encode the data)
                    barcodeType = TYPE.CODE128; // dynamic
                }
                else if (barcode.Mode == "D")
                {
                    //D (UCC/EAN mode, field data must contain GS1 numbers)
                    barcodeType = TYPE.CODE128C;
                    
                    if (!content.StartsWith(FNC1))
                    {
                        content = FNC1 + content;
                    }
                }
                else if (barcode.Mode == "U")
                {
                    //U (UCC case mode, field data must contain 19 digits)
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
