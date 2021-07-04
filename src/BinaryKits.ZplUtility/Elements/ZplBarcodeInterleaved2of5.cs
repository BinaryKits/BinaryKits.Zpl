using System;
using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// Interleaved 2 of 5 Barcode 
    /// </summary>
    public class ZplBarcodeInterleaved2of5 : ZplBarcode
    {
        public bool Mod10CheckDigit { get; private set; }

        /// <summary>
        /// Interleaved 2 of 5 Barcode 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        /// <param name="mod10CheckDigit"></param>
        public ZplBarcodeInterleaved2of5(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool mod10CheckDigit = false)
            : base(content, positionX, positionY, height, fieldOrientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            if (!IsDigitsOnly(content))
            {
                throw new ArgumentException("Interleaved 2 of 5 Barcode allow only digits", nameof(content));
            }

            Mod10CheckDigit = mod10CheckDigit;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^B2{RenderFieldOrientation()},{context.Scale(Height)},{(PrintInterpretationLine ? "Y" : "N")},{(PrintInterpretationLineAboveCode ? "Y" : "N")},{(Mod10CheckDigit ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
