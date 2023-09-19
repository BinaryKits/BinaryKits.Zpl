using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using ZXing;
using ZXing.Datamatrix;
using ZXing.Datamatrix.Encoder;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for Data Matrix Barcode Elements
    /// </summary>
    public class DataMatrixElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplDataMatrix;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplDataMatrix dataMatrix)
            {
                if (dataMatrix.Height == 0)
                    throw new System.Exception("Matrix Height is set to zero.");

                if (string.IsNullOrWhiteSpace(dataMatrix.Content))
                    throw new System.Exception("Matrix Content is empty.");

                float x = dataMatrix.PositionX;
                float y = dataMatrix.PositionY;

                var writer = new DataMatrixWriter();
                var encodingOptions = new DatamatrixEncodingOptions();
                encodingOptions.SymbolShape = SymbolShapeHint.FORCE_SQUARE;

                var result = writer.encode(dataMatrix.Content, BarcodeFormat.DATA_MATRIX, 0, 0, encodingOptions.Hints);

                using var resizedImage = this.BitMatrixToSKBitmap(result, dataMatrix.Height);
                {
                    var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                    this.DrawBarcode(png, x, y, resizedImage.Width, resizedImage.Height, dataMatrix.FieldOrigin != null, dataMatrix.FieldOrientation);
                }
            }
        }
    }
}
