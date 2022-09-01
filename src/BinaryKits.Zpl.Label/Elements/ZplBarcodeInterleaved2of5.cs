using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
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
        /// <param name="moduleWidth"></param>
        /// <param name="wideBarToNarrowBarWidthRatio"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        /// <param name="mod10CheckDigit"></param>
        /// <param name="bottomToTop"></param>
        public ZplBarcodeInterleaved2of5(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            int moduleWidth = 2,
            double wideBarToNarrowBarWidthRatio = 3,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool mod10CheckDigit = false,
            bool bottomToTop = false)
            : base(content,
                  positionX,
                  positionY,
                  height,
                  moduleWidth,
                  wideBarToNarrowBarWidthRatio,
                  fieldOrientation,
                  printInterpretationLine,
                  printInterpretationLineAboveCode,
                  bottomToTop)
        {
            if (!IsDigitsOnly(content))
            {
                throw new ArgumentException("Interleaved 2 of 5 Barcode allow only digits", nameof(content));
            }

            Mod10CheckDigit = mod10CheckDigit;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderModuleWidth());
            result.Add($"^B2{RenderFieldOrientation()},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()},{(Mod10CheckDigit ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
