using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.ZplUtility.Elements
{
    //^FD – Field Data
    public class ZplTextField : ZplPositionedElementBase
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
        /// Constuct a ^FD (Field Data) element, together with the ^FO, ^A and ^FH.
        /// Control character will be handled (Conver to Hex or replace with ' ')
        /// </summary>
        /// <param name="text">Original text content</param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="font"></param>
        /// <param name="newLineConversion"></param>
        /// <param name="useHexadecimalIndicator"></param>
        /// <param name="reversePrint"></param>
        public ZplTextField(string text, int positionX, int positionY, ZplFont font, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true, bool reversePrint = false) : base(positionX, positionY)
        {
            Text = text;
            Origin = new ZplOrigin(positionX, positionY);
            Font = font;
            UseHexadecimalIndicator = useHexadecimalIndicator;
            NewLineConversion = newLineConversion;
            ReversePrint = reversePrint;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
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
            
            sb.Append("^FD");
            foreach (var c in Text)
            {
                sb.Append(SanitizeCharacter(c));
            }

            sb.Append("^FS");

            return sb.ToString();
        }

        private string SanitizeCharacter(char input)
        {
            if (UseHexadecimalIndicator)
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
                switch (NewLineConversion)
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
    }
}
