using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    //^FB – Field Block
    public class ZplFieldBlock : ZplTextField
    {
        public int Width { get; private set; }

        public int MaxLineCount { get; private set; }

        public int LineSpace { get; private set; }

        public TextJustification TextJustification { get; private set; }

        //hanging indent (in dots) of the second and remaining lines
        public int HangingIndent { get; private set; }

        public ZplFieldBlock(
            string text,
            int positionX,
            int positionY,
            int width,
            ZplFont font,
            int maxLineCount = 1,
            int lineSpace = 0,
            TextJustification textJustification = TextJustification.Left,
            int hangingIndent = 0,
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToZplNewLine,
            bool useHexadecimalIndicator = true,
            bool reversePrint = false,
            bool bottomToTop = false)
            : base(text, positionX, positionY, font, newLineConversion, useHexadecimalIndicator, reversePrint, bottomToTop)
        {
            TextJustification = textJustification;
            Width = width;
            MaxLineCount = maxLineCount;
            LineSpace = lineSpace;
            HangingIndent = hangingIndent;
        }

        private string RenderTextJustification()
        {
            switch (TextJustification)
            {
                case TextJustification.Left:
                    return "L";
                case TextJustification.Center:
                    return "C";
                case TextJustification.Right:
                    return "R";
                case TextJustification.Justified:
                    return "J";
            }

            throw new NotImplementedException("Unknown Text Justification");
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ XA
            //^ CF0,30,30 ^ FO25,50
            //   ^ FB250,4,,
            //^ FDFD command that IS\&
            // preceded by an FB \&command.
            // ^ FS
            // ^ XZ
            var result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(RenderPosition(context));
            result.Add($"^FB{context.Scale(Width)},{MaxLineCount},{context.Scale(LineSpace)},{RenderTextJustification()},{context.Scale(HangingIndent)}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
