using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    //^FD – Field Data
    public class ZplTextField : ZplPositionedElementBase, IFormatElement
    {
        //^A
        public ZplFont Font { get; protected set; }
        //^FH
        public bool UseHexadecimalIndicator { get; protected set; }
        //^FR
        public bool ReversePrint { get; protected set; }

        public NewLineConversionMethod NewLineConversion { get; protected set; }
        //^FD
        public string Text { get; protected set; }

        /// <summary>
        /// Construct a ^FD (Field Data) element, together with the ^FO, ^A and ^FH.
        /// Control character will be handled (Convert to Hex or replace with ' ')
        /// </summary>
        /// <param name="text">Original text content</param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="font"></param>
        /// <param name="newLineConversion"></param>
        /// <param name="useHexadecimalIndicator"></param>
        /// <param name="reversePrint"></param>
        /// <param name="bottomToTop"></param>
        public ZplTextField(
            string text,
            int positionX,
            int positionY,
            ZplFont font,
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace,
            bool useHexadecimalIndicator = true,
            bool reversePrint = false,
            bool bottomToTop = false)
            : base(positionX, positionY, bottomToTop)
        {
            Text = text;
            Font = font;
            UseHexadecimalIndicator = useHexadecimalIndicator;
            NewLineConversion = newLineConversion;
            ReversePrint = reversePrint;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(RenderPosition(context));
            result.Add(RenderFieldDataSection());

            return result;
        }

        protected string RenderFieldDataSection()
        {
            var sb = new StringBuilder();
            if (UseHexadecimalIndicator)
            {
                sb.Append("^FH");
            }
            if (ReversePrint)
            {
                sb.Append("^FR");
            }

            if (Text != null)
            {
                sb.Append("^FD");
                foreach (var c in Text)
                {
                    sb.Append(SanitizeCharacter(c, NewLineConversion, UseHexadecimalIndicator));
                }

                sb.Append("^FS");
            }

            return sb.ToString();
        }

        internal static string SanitizeCharacter(
            char input,
            NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace,
            bool useHexadecimalIndicator = true)
        {
            if (useHexadecimalIndicator)
            {
                //Convert to hex
                switch (input)
                {
                    case '_':
                    case '^':
                    case '~':
                        return "_" + Convert.ToByte(input).ToString("X2");
                    case '\\':
                        return " ";
                }
            }
            else
            {
                //The field data can be any printable character except those used as command prefixes(^ and ~).
                //Replace '^', '~'
                switch (input)
                {
                    case '^':
                    case '~':
                    case '\\':
                        return " ";
                }
            }

            if (input == '\n')
            {
                switch (newLineConversion)
                {
                    case NewLineConversionMethod.ToEmpty:
                        return "";
                    case NewLineConversionMethod.ToSpace:
                        return " ";
                    case NewLineConversionMethod.ToZplNewLine:
                        return @"\&";
                }
            }

            return input.ToString();
        }

        /// <inheritdoc />
        public void SetTemplateContent(string content)
        {
            Text = content;
        }
    }
}
