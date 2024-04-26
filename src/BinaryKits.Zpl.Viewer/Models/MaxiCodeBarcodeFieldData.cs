using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class MaxiCodeBarcodeFieldData : FieldDataBase
    {
        public int Mode { get; set; }
        public int Position { get; set; }
        public int Total { get; set; }
        public bool UseHexadecimalIndicator { get; set; }
    }
}
