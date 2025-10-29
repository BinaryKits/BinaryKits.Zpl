using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class AztecBarcodeFieldData : FieldDataBase
    {
        public FieldOrientation FieldOrientation { get; set; }
        public int MagnificationFactor { get; set; }
        public bool ExtendedChannel { get; set; }
        public int ErrorControl { get; set; }
        public bool MenuSymbol { get; set; }
        public int SymbolCount { get; set; }
        public string IdField { get; set; }

    }
}
