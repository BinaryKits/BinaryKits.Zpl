using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Code 128 Barcode
    /// </summary>
    public class ZplBarcode128 : ZplBarcode
    {
        /// <summary>
        /// Code 128 Barcode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        public ZplBarcode128(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, fieldOrientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(FieldOrigin.Render(context));
            result.Add($"^BC{RenderFieldOrientation()},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
