using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Symologies;
using SkiaSharp;
using System;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Code 128 Barcode elements
    /// </summary>
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {
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
                string content = barcode.Content;
                Code128CodeSet codeSet = Code128CodeSet.Code128B;
                bool gs1 = false;
                if (string.IsNullOrEmpty(barcode.Mode) || barcode.Mode == "N")
                {
                    codeSet = Code128CodeSet.Code128B;
                }
                else if (barcode.Mode == "A")
                {
                    codeSet = Code128CodeSet.Code128;
                }
                else if (barcode.Mode == "D")
                {
                    codeSet = Code128CodeSet.Code128;
                    gs1 = true;
                }
                else if (barcode.Mode == "U")
                {
                    codeSet = Code128CodeSet.Code128C;
                    content = content.PadLeft(19, '0').Substring(0, 19);
                    int checksum = 0;
                    for (int i = 0; i < 19; i++)
                    {
                        checksum += (content[i] - 48) * (i % 2 * 2 + 7);
                    }
                    content = $">8{content}{checksum % 10}";
                }

                float x = barcode.PositionX;
                float y = barcode.PositionY;

                var (data, interpretation) = ZplCode128Symbology.Encode(content, codeSet, gs1);
                using var resizedImage = this.BoolArrayToSKBitmap(data.ToArray(), barcode.Height, barcode.ModuleWidth);
                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    // TODO: use font 0, auto scale for Mode D
                    float labelFontSize = Math.Min(barcode.ModuleWidth * 10f, 100f);
                    var labelTypeFace = options.FontLoader("A");
                    var labelFont = new SKFont(labelTypeFace, labelFontSize);
                    this.DrawInterpretationLine(interpretation, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.PrintInterpretationLineAboveCode, options);
                }
            }
        }

    }
}
