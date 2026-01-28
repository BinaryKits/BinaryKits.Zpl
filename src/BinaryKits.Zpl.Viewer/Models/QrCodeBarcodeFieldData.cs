using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class QrCodeBarcodeFieldData : FieldDataBase
    {
        public int Model { get; set; }
        public FieldOrientation FieldOrientation { get; set; }
        public int MagnificationFactor { get; set; }
        public ErrorCorrectionLevel ErrorCorrection { get; set; }
        public int MaskValue { get; set; }
    }
}
