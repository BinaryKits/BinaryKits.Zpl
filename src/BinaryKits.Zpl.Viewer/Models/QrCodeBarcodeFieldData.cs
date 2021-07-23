using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class QrCodeBarcodeFieldData : FieldDataBase
    {
        public FieldOrientation FieldOrientation { get; set; }
        public int MagnificationFactor { get; set; }
        public string ErrorCorrection { get; set; }
        public int MaskValue { get; set; }
    }
}
