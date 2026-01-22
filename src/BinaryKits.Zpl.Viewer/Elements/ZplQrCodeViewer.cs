using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.Elements
{
    internal class ZplQrCodeViewer : ZplQrCode
    {
        internal int VerticalQuietZone { get; private set; }

        public ZplQrCodeViewer(
            string content,
            int positionX,
            int positionY,
            int model,
            int magnificationFactor,
            ErrorCorrectionLevel errorCorrectionLevel,
            int maskValue,
            FieldOrientation fieldOrientation,
            char? hexadecimalIndicator,
            bool bottomToTop,
            bool useDefaultPosition,
            int verticalQuietZone)
            : base(content, positionX, positionY, model, magnificationFactor, errorCorrectionLevel, maskValue, fieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition)
        {
            this.VerticalQuietZone = verticalQuietZone;
        }
    }
}
