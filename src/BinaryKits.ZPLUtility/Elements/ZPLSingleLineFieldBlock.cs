namespace BinaryKits.ZPLUtility.Elements
{
    //Similar to ZPLTextField with big line spacing, so only the first line is visible
    public class ZPLSingleLineFieldBlock : ZPLFieldBlock
    {
        public ZPLSingleLineFieldBlock(
            string text,
            int positionX,
            int positionY,
            int width,
            ZPLFont font,
            string textJustification = "L",
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace,
            bool useHexadecimalIndicator = true,
            bool reversePrint = false)
            : base(text, positionX, positionY, width, font, 9999, 9999, textJustification, 0, newLineConversion, useHexadecimalIndicator, reversePrint)
        {
        }
    }
}
