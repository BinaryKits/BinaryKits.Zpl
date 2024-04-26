using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Code 93 Barcode
    /// </summary>
    public class ZplBarcode93 : ZplBarcode
    {
        public bool CheckDigit { get; private set; }

        /// <summary>
        /// Code 93 Barcode
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
        /// <param name="bottomToTop"></param>
        /// <param name="checkDigit"></param>
        public ZplBarcode93(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            int moduleWidth = 2,
            double wideBarToNarrowBarWidthRatio = 3,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool checkDigit = false,
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
            this.CheckDigit = checkDigit;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //TODO:Add 'mode'

            //^FO100,100 ^ BY3
            //^BAN,100,Y,N,N
            //^FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderModuleWidth());
            result.Add($"^BA{RenderFieldOrientation()},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()},{(CheckDigit ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
