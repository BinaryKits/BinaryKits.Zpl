using System;
using System.Collections.Generic;

namespace BinaryKits.ZPLUtility.Elements
{
    /// <summary>
    /// EAN-13
    /// </summary>
    public class ZPLBarcodeEan13 : ZPLBarcode
    {
        public ZPLBarcodeEan13(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            if (!IsDigitsOnly(content))
            {
                throw new ArgumentException("EAN-13 Barcode allow only digits", nameof(content));
            }
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^BE{Orientation},{context.Scale(Height)},{(PrintInterpretationLine ? "Y" : "N")},{(PrintInterpretationLineAboveCode ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
