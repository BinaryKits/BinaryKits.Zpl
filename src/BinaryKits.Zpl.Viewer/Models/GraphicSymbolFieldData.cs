using BinaryKits.Zpl.Label;

using System;
namespace BinaryKits.Zpl.Viewer.Models
{
    public class GraphicSymbolFieldData : FieldDataBase
    {
        public FieldOrientation FieldOrientation { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
