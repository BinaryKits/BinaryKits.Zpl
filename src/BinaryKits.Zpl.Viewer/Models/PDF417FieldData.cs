using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class PDF417FieldData : FieldDataBase
    {
        public int Height { get; set; }
        public FieldOrientation FieldOrientation { get; set; }
        public int? Columns { get; set; }
        public int? Rows { get; set; }
        public bool Compact { get; set; }
        public int SecurityLevel { get; set; }
    }
}
