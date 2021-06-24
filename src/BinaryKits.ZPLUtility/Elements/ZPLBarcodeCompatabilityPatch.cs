using System;

/// <summary>
/// Keep the old "ZPLBarCode" class names, prevent breaking change at user side
/// </summary>
namespace BinaryKits.ZPLUtility.Elements
{
    [Obsolete("ZPLBarCode39 is deprecated, please use ZPLBarcode39 instead.")]
    public class ZPLBarCode39 : ZPLBarcode39
    {
        public ZPLBarCode39(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod43CheckDigit = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode, mod43CheckDigit)
        {
        }
    }

    [Obsolete("ZPLBarCode128 is deprecated, please use ZPLBarcode128 instead.")]
    public class ZPLBarCode128 : ZPLBarcode128
    {
        public ZPLBarCode128(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false)
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode)
        {
        }
    }

    [Obsolete("ZPLBarCodeANSICodabar is deprecated, please use ZPLBarcodeANSICodabar instead.")]
    public class ZPLBarCodeANSICodabar : ZPLBarcodeANSICodabar
    {
        public ZPLBarCodeANSICodabar(string content, int positionX, int positionY, int height, char startCharacter, char stopCharacter, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool checkDigit = false) 
            : base(content, positionX, positionY, height, startCharacter, stopCharacter, orientation, printInterpretationLine, printInterpretationLineAboveCode, checkDigit)
        {
        }
    }

    public class ZPLBarCodeInterleaved2of5 : ZPLBarcodeInterleaved2of5
    {
        public ZPLBarCodeInterleaved2of5(string content, int positionX, int positionY, int height = 100, string orientation = "N", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false, bool mod10CheckDigit = false) 
            : base(content, positionX, positionY, height, orientation, printInterpretationLine, printInterpretationLineAboveCode, mod10CheckDigit)
        {
        }
    }
}
