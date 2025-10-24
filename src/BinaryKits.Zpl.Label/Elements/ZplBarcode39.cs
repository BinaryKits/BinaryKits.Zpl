using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Code 39 Barcode
    /// </summary>
    public class ZplBarcode39 : ZplBarcode
    {
        public bool Mod43CheckDigit { get; private set; }

        /// <summary>
        /// Code 39 Barcode
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
        /// <param name="mod43CheckDigit"></param>
        /// <param name="bottomToTop"></param>
        /// <param name="useDefaultPosition"></param>
        public ZplBarcode39(
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
            bool mod43CheckDigit = false,
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
            Mod43CheckDigit = mod43CheckDigit;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderModuleWidth());
            result.Add($"^B3{RenderFieldOrientation()},{RenderBoolean(Mod43CheckDigit)},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
