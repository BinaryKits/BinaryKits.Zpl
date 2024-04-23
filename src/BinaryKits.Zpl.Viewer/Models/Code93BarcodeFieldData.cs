﻿using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    internal class Code93BarcodeFieldData : FieldDataBase
    {
        public FieldOrientation FieldOrientation { get; set; }
        public int Height { get; set; }
        public bool PrintInterpretationLine { get; set; }
        public bool PrintInterpretationLineAboveCode { get; set; }
        public bool PrintCheckDigit { get; set; }
    }
}
