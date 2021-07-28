using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Ansi Codabar Barcode
    /// </summary>
    public class ZplBarcodeAnsiCodabar : ZplBarcode
    {
        public bool CheckDigit { get; private set; }
        public char StartCharacter { get; private set; }
        public char StopCharacter { get; private set; }

        /// <summary>
        /// Ansi Codabar Barcode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="startCharacter">A,B,C,D</param>
        /// <param name="stopCharacter">A,B,C,D</param>
        /// <param name="fieldOrientation"></param>
        /// <param name="printInterpretationLine"></param>
        /// <param name="printInterpretationLineAboveCode"></param>
        /// <param name="checkDigit"></param>
        public ZplBarcodeAnsiCodabar(
            string content,
            int positionX,
            int positionY,
            int height,
            char startCharacter,
            char stopCharacter,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool printInterpretationLine = true,
            bool printInterpretationLineAboveCode = false,
            bool checkDigit = false)
            : base(content, positionX, positionY, height, fieldOrientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            if (!IsValidCharacter(startCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar start charactor must be one of A, B, C, D");
            }

            if (!IsValidCharacter(stopCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar stop charactor must be one of A, B, C, D");
            }

            CheckDigit = checkDigit;
            StartCharacter = char.ToUpper(startCharacter);
            StopCharacter = char.ToUpper(stopCharacter);
        }

        private bool IsValidCharacter(char character)
        {
            var chars = new[] { 'A', 'B', 'C', 'D' };
            return chars.Contains(char.ToUpperInvariant(character));
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO100,100 ^ BY3
            // ^ BKN,N,150,Y,N,A,A
            //  ^ FD123456 ^ FS
            var result = new List<string>();
            result.AddRange(FieldOrigin.Render(context));
            result.Add($"^BK{RenderFieldOrientation()},{(CheckDigit ? "Y" : "N")},{context.Scale(Height)},{RenderPrintInterpretationLine()},{RenderPrintInterpretationLineAboveCode()},{StartCharacter},{StopCharacter}");
            result.Add($"^FD{Content}^FS");

            return result;
        }
    }
}
