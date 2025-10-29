using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// UPC-E Barcode
    /// </summary>
    public class ZplBarcodeUpcExtension : ZplBarcode
    {
        /// <summary>
        /// UPC Extension Barcode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="moduleWidth"></param>
        /// <param name="wideBarToNarrowBarWidthRatio"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="hexadecimalIndicator"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        /// <param name="bottomToTop"></param>
        /// <param name="useDefaultPosition"></param>
        public ZplBarcodeUpcExtension(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            int moduleWidth = 2,
            double wideBarToNarrowBarWidthRatio = 3,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            char? hexadecimalIndicator = null,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(content,
                  positionX,
                  positionY,
                  height,
                  moduleWidth,
                  wideBarToNarrowBarWidthRatio,
                  fieldOrientation,
                  hexadecimalIndicator,
                  printInterpretationLine,
                  printInterpretationLineAboveCode,
                  bottomToTop,
                  useDefaultPosition)
        {
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            List<string> result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderModuleWidth());
            result.Add($"^BS{RenderFieldOrientation()},{context.Scale(this.Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
