using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System;
using ZXing.OneD;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode93ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode93;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcode93 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                var content = barcode.Content;

                var writer = new Code93Writer();
                var result = writer.encode(content);
                using var resizedImage = this.BoolArrayToSKBitmap(result, barcode.Height, barcode.ModuleWidth);
                var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation);

                if (barcode.PrintInterpretationLine)
                {
                    float labelFontSize = Math.Min(barcode.ModuleWidth * 10f, 100f);
                    var labelTypeFace = options.FontLoader("A");
                    var labelFont = new SKFont(labelTypeFace, labelFontSize);
                    this.DrawInterpretationLine(content, labelFont, x, y, resizedImage.Width, resizedImage.Height, barcode.FieldOrigin != null, barcode.FieldOrientation, barcode.PrintInterpretationLineAboveCode, options);
                }

            }
        }
    }
}
