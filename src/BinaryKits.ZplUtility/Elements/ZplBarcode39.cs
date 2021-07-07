using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
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
        /// <param name="fieldOrientation"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        /// <param name="mod43CheckDigit"></param>
        public ZplBarcode39(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool mod43CheckDigit = false)
            : base(content, positionX, positionY, height, fieldOrientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            Mod43CheckDigit = mod43CheckDigit;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^B3{RenderFieldOrientation()},{(Mod43CheckDigit ? "Y" : "N")},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
