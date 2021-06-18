using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    /// <summary>
    /// The ^TB command prints a text block with defined width and height.
    /// The text block has an automatic word-wrap function. 
    /// If the text exceeds the block height, the text is truncated. Does not support \n
    /// </summary>
    public class ZPLTextBlock : ZPLTextField
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public ZPLTextBlock(
            string text,
            int positionX,
            int positionY,
            int width,
            int height,
            ZPLFont font,
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace,
            bool useHexadecimalIndicator = true)
            : base(text, positionX, positionY, font, newLineConversion, useHexadecimalIndicator)
        {
            Width = width;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add($"^TB{Font.Orientation},{context.Scale(Width)},{context.Scale(Height)}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
