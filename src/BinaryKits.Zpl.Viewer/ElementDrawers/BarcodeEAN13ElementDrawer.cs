using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using System.Drawing;


namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class BarcodeEAN13ElementDrawer : BarcodeDrawerBase
    {
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcodeEan13;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplBarcodeEan13 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                if (barcode.FieldTypeset != null)
                {
                    y -= barcode.Height;
                }

                var barcodeElement = new Barcode
                {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height                
                };

                var content = barcode.Content;
                if(content.Length < 12)
                {
                    var number = long.Parse(content);
                    content = number.ToString("D12");
                }else if(content.Length > 12)
                {
                    content = content.Substring(content.Length - 12, 12);
                }
                using var image = barcodeElement.Encode(TYPE.EAN13, content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height, image.Width, barcode.FieldOrigin != null, x, y, barcode.FieldOrientation);
            }
        }
    }
}
