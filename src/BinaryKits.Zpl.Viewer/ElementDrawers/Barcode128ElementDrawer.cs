using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;
using System.Drawing;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode128ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode128;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element) {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcode128 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                float labelFontSize = barcode.ModuleWidth * 7.25f;
                var labelTypeface = options.FontLoader("1");
                var labelFont = new SKFont(labelTypeface, labelFontSize);
                labelFont.GetFontMetrics(out var labelFontMetrics);
                int labelHeight = (int)Math.Ceiling((labelFontMetrics.Bottom - labelFontMetrics.Top) + labelFontMetrics.Descent);

                var barcodeElement = new Barcode {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height + labelHeight,
                    IncludeLabel = barcode.PrintInterpretationLine,
                    LabelPosition = barcode.PrintInterpretationLineAboveCode ? LabelPositions.TOPCENTER : LabelPositions.BOTTOMCENTER,
                    LabelFont = labelFont.ToSystemDrawingFont()
                };

                using var image = barcodeElement.Encode(TYPE.CODE128B, barcode.Content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height + labelHeight, image.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
