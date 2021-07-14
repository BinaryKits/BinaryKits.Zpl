namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Similar to ZplTextField with big line spacing, so only the first line is visible
    /// </summary>
    public class ZplSingleLineFieldBlock : ZplFieldBlock
    {
        public ZplSingleLineFieldBlock(
            string text,
            int positionX,
            int positionY,
            int width,
            ZplFont font,
            TextJustification textJustification = TextJustification.Left,
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace,
            bool useHexadecimalIndicator = true,
            bool reversePrint = false)
            : base(text, positionX, positionY, width, font, 9999, 9999, textJustification, 0, newLineConversion, useHexadecimalIndicator, reversePrint)
        {
        }
    }
}
