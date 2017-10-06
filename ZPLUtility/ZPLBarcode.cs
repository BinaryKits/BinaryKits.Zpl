using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
    public abstract class ZPLBarcode : ZPLPositionedElementBase
    {
        public ZPLBarcode(string content, int positionX, int positionY, int height, string orientation, bool printInterpretationLine, bool printInterpretationLineAboveCode) : base(positionX, positionY)
        {
            Origin = new ZPLOrigin(positionX, positionY);
            Content = content;
            Height = height;
            Orientation = orientation;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        public int Height { get; protected set; }

        public string Orientation { get; protected set; }

        public string Content { get; protected set; }
        public bool PrintInterpretationLine { get; protected set; }
        public bool PrintInterpretationLineAboveCode { get; protected set; }
    }

    public class ZPLBarCode128 : ZPLBarcode
    {
        public ZPLBarCode128(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^BC" + Orientation + ","
                + context.Scale(Height) + ","
                + (PrintInterpretationLine ? "Y" : "N") + ","
                + (PrintInterpretationLineAboveCode ? "Y" : "N"));
            result.Add("^FD" + Content + "^FS");

            return result;
        }
    }

    public class ZPLBarCode39 : ZPLBarcode
    {
        public bool Mod43CheckDigit { get; private set; }

        public ZPLBarCode39(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod43CheckDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            Mod43CheckDigit = mod43CheckDigit;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^FO100,100 ^ BY3
            //^BCN,100,Y,N,N
            //^FD123456 ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^B3" + Orientation + ","
                + (Mod43CheckDigit ? "Y" : "N") + ","
                + context.Scale(Height) + ","
                + (PrintInterpretationLine ? "Y" : "N") + ","
                + (PrintInterpretationLineAboveCode ? "Y" : "N"));
            result.Add("^FD" + Content + "^FS");

            return result;
        }
    }

    public class ZPLBarCodeANSICodabar : ZPLBarcode
    {
        public bool CheckDigit { get; private set; }
        public char StartCharacter { get; private set; }
        public char StopCharacter { get; private set; }

        public ZPLBarCodeANSICodabar(string content, int positionX, int positionY, int height, char startCharacter, char stopCharacter, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool checkDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
            CheckDigit = checkDigit;
            if (!"ABCDabcd".Contains(startCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar start charactor must be one of A, B, C D");
            }
            StartCharacter = Char.ToUpper(startCharacter);
            if (!"ABCDabcd".Contains(stopCharacter))
            {
                throw new InvalidOperationException("ANSI Codabar stop charactor must be one of A, B, C D");
            }
            StopCharacter = Char.ToUpper(stopCharacter);
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^ FO100,100 ^ BY3
            // ^ BKN,N,150,Y,N,A,A
            //  ^ FD123456 ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^BK" + Orientation + ","
                + (CheckDigit ? "Y" : "N") + ","
                + context.Scale(Height) + ","
                + (PrintInterpretationLine ? "Y" : "N") + ","
                + (PrintInterpretationLineAboveCode ? "Y" : "N") + ","
                + StartCharacter + ","
                + StopCharacter);
            result.Add("^FD" + Content + "^FS");

            return result;
        }
    }
}
