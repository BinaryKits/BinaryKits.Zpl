using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Code 128 Barcode
    /// </summary>
    public class ZplBarcode128 : ZplBarcode
    {

        public string Mode { get; set; }

        /// <summary>
        /// Code 128 Barcode
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
        /// <param name="mode"></param>
        public ZplBarcode128(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            int moduleWidth = 2,
            double wideBarToNarrowBarWidthRatio = 3,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool bottomToTop = false,
            string mode = "N")
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
            this.Mode = mode;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //TODO:Add 'UCC check digit' and 'mode'

            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderModuleWidth());
            result.Add($"^BC{RenderFieldOrientation()},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
