using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplBarcodeAnsiCodabar : ZplBarcode
    {
        public bool CheckDigit { get; private set; }
        public char StartCharacter { get; private set; }
        public char StopCharacter { get; private set; }

        public ZplBarcodeAnsiCodabar(
            string content,
            int positionX,
            int positionY,
            int height,
            char startCharacter,
            char stopCharacter,
            string orientation = "N",
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool checkDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            CheckDigit = checkDigit;
            if (!"ABCDabcd".Contains(startCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar start charactor must be one of A, B, C D");
            }
            StartCharacter = char.ToUpper(startCharacter);
            if (!"ABCDabcd".Contains(stopCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar stop charactor must be one of A, B, C D");
            }
            StopCharacter = char.ToUpper(stopCharacter);
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO100,100 ^ BY3
            // ^ BKN,N,150,Y,N,A,A
            //  ^ FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^BK{Orientation},{(CheckDigit ? "Y" : "N")},{context.Scale(Height)},{(PrintInterpretationLine ? "Y" : "N")},{(PrintInterpretationLineAboveCode ? "Y" : "N")},{StartCharacter},{StopCharacter}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
