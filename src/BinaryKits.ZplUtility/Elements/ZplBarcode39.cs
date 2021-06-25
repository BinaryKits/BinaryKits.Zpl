using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// Code 39
    /// </summary>
    public class ZplBarcode39 : ZplBarcode
    {
        public bool Mod43CheckDigit { get; private set; }

        public ZplBarcode39(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod43CheckDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
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
            result.Add($"^B3{Orientation},{(Mod43CheckDigit ? "Y" : "N")},{context.Scale(Height)},{(PrintInterpretationLine ? "Y" : "N")},{(PrintInterpretationLineAboveCode ? "Y" : "N")}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
