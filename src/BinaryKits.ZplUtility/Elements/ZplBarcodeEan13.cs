using System;
using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// EAN-13
    /// </summary>
    public class ZplBarcodeEan13 : ZplBarcode
    {
        public ZplBarcodeEan13(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            if (!IsDigitsOnly(content))
            {
                throw new ArgumentException("EAN-13 Barcode allow only digits", nameof(content));
            }
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^BE{Orientation},{context.Scale(Height)},{(PrintInterpretationLine ? "Y" : "N")},{(PrintInterpretationLineAboveCode ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
