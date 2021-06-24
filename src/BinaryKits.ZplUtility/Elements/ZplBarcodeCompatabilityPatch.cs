using System;

/// <summary>
/// Keep the old "ZPLBarCode" class names, prevent breaking change at user side
/// </summary>
namespace BinaryKits.ZplUtility.Elements
{
    [Obsolete("ZPLBarCode39 is deprecated, please use ZPLBarcode39 instead.")]
    public class ZplBarCode39 : ZplBarcode39
    {
        public ZplBarCode39(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod43CheckDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode, mod43CheckDigit)
        {
        }
    }

    [Obsolete("ZPLBarCode128 is deprecated, please use ZPLBarcode128 instead.")]
    public class ZplBarCode128 : ZplBarcode128
    {
        public ZplBarCode128(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
        }
    }

    [Obsolete("ZPLBarCodeANSICodabar is deprecated, please use ZPLBarcodeANSICodabar instead.")]
    public class ZplBarCodeAnsiCodabar : ZplBarcodeAnsiCodabar
    {
        public ZplBarCodeAnsiCodabar(string content, int positionX, int positionY, int height, char startCharacter, char stopCharacter, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool checkDigit = false) 
            : base(content, positionX, positionY, height, startCharacter, stopCharacter, orientation, printInterpretationLine, printInterpretationLineAboveCode, checkDigit)
        {
        }
    }

    public class ZplBarCodeInterleaved2of5 : ZplBarcodeInterleaved2of5
    {
        public ZplBarCodeInterleaved2of5(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod10CheckDigit = false) 
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode, mod10CheckDigit)
        {
        }
    }
}
