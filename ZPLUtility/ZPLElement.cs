using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
    public abstract class ZPLElementBase
    {
        public List<string> Comments { get; protected set; }

        //Indicate the rendering process whether this elemenet can be skipped
        public bool IsEnabled { get; set; }

        //Optionally identify the element for future lookup/manipulation
        public string Id { get; set; }

        public ZPLElementBase()
        {
            Comments = new List<string>();
            IsEnabled = true;
        }

        public IEnumerable<string> Render()
        {
            return Render(ZPLRenderOptions.DefaultOptions);
        }

        public string RenderToString()
        {
            return string.Join(" ", Render());
        }

        public abstract IEnumerable<string> Render(ZPLRenderOptions context);

        public string ToZPLString()
        {
            return ToZPLString(ZPLRenderOptions.DefaultOptions);
        }

        public string ToZPLString(ZPLRenderOptions context)
        {
            return string.Join("\n", Render(context));
        }
    }

    public abstract class ZPLPositionedElementBase : ZPLElementBase
    {
        public ZPLOrigin Origin { get; protected set; }

        public ZPLPositionedElementBase(int positionX, int positionY) : base()
        {
            Origin = new ZPLOrigin(positionX, positionY);
        }
    }

    public class ZPLRaw : ZPLElementBase
    {
        public string RawContent { get; set; }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            return new[] { RawContent };
        }

        public ZPLRaw(string rawZPLCode)
        {
            RawContent = rawZPLCode;
        }
    }

    //^A – Scalable/Bitmapped Font
    public class ZPLFont : ZPLElementBase
    {
        public string FontName { get; private set; }
        public string Orientation { get; private set; }
        public int FontWidth { get; private set; }
        public int FontHeight { get; private set; }

        public ZPLFont(int fontWidth = 30, int fontHeight = 30, string fontName = "0", string orientation = "N")
        {
            FontName = fontName;
            Orientation = orientation;
            FontWidth = fontWidth;
            FontHeight = fontHeight;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            string textOrientation = Orientation;
            if (string.IsNullOrEmpty(textOrientation))
            {
                textOrientation = context.DefaultTextOrientation;
            }
            return new[] { "^A" + FontName + textOrientation + "," + context.Scale(FontHeight) + "," + context.Scale(FontWidth) };
        }
    }

    public class ZPLBarCodeFieldDefault : ZPLElementBase
    {
        public int ModuleWidth { get; private set; }

        public double BarWidthRatio { get; private set; }

        public int Height { get; private set; }

        public ZPLBarCodeFieldDefault(int moduleWidth = 2, double barWidthRatio = 3.0d, int height = 10)
        {
            ModuleWidth = moduleWidth;
            BarWidthRatio = barWidthRatio;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            return new[] { "^BY" + context.Scale(ModuleWidth) + "," + Math.Round(BarWidthRatio, 1) + "," + context.Scale(Height) };
        }
    }

    //^FO – Field Origin
    public class ZPLOrigin : ZPLElementBase
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }

        public ZPLOrigin(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^ FO50,50
            return new string[] { "^FO" + context.Scale(PositionX) + "," + context.Scale(PositionY) };
        }
    }

    //^FD – Field Data
    public class ZPLTextField : ZPLPositionedElementBase
    {
        //^A
        public ZPLFont Font { get; protected set; }
        //^FH
        public bool UseHexadecimalIndicator { get; protected set; }
        //^FR
        public bool ReversePrint { get; protected set; }

        public NewLineConversionMethod NewLineConversion { get; protected set; }
        //^FD
        public string Text { get; protected set; }

        /// <summary>
        /// Constuct a ^FD (Field Data) element, together with the ^FO, ^A and ^FH.Control character will be handled (Conver to Hex or replace with ' ')
        /// </summary>
        /// <param name="text">Original text content</param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="fontWidth"></param>
        /// <param name="fontHeight"></param>
        /// <param name="fontName"></param>
        /// <param name="orientation"></param>
        /// <param name="useHexadecimalIndicator"></param>
        public ZPLTextField(string text, int positionX, int positionY, ZPLFont font, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true, bool reversePrint = false) : base(positionX, positionY)
        {
            Text = text;
            Origin = new ZPLOrigin(positionX, positionY);
            Font = font;
            UseHexadecimalIndicator = useHexadecimalIndicator;
            NewLineConversion = newLineConversion;
            ReversePrint = reversePrint;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add(RenderFieldDataSection());

            return result;
        }

        protected string RenderFieldDataSection()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(UseHexadecimalIndicator ? "^FH" : "");
            sb.Append(ReversePrint ? "^FR" : "");
            sb.Append("^FD");
            foreach (var c in Text)
            {
                sb.Append(SanitizeCharacter(c));
            }
            sb.Append("^FS");

            return sb.ToString();
        }

        string SanitizeCharacter(char input)
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
                    case NewLineConversionMethod.ToZPLNewLine:
                        return @"\&";
                }
            }

            return input.ToString();
        }

        public enum NewLineConversionMethod
        {
            ToSpace,
            ToEmpty,
            ToZPLNewLine,
        }
    }

    //^FB – Field Block
    public class ZPLFieldBlock : ZPLTextField
    {
        public int Width { get; private set; }

        public int MaxLineCount { get; private set; }

        public int LineSpace { get; private set; }

        public string TextJustification { get; private set; }

        //hanging indent (in dots) of the second and remaining lines
        public int HangingIndent { get; private set; }

        public ZPLFieldBlock(string text, int positionX, int positionY, int width, ZPLFont font, int maxLineCount = 1, int lineSpace = 0, string textJustification = "L", int hangingIndent = 0, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToZPLNewLine, bool useHexadecimalIndicator = true, bool reversePrint = false)
            : base(text, positionX, positionY, font, newLineConversion, useHexadecimalIndicator, reversePrint)
        {
            TextJustification = textJustification;
            Width = width;
            MaxLineCount = maxLineCount;
            LineSpace = lineSpace;
            HangingIndent = hangingIndent;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^ XA
            //^ CF0,30,30 ^ FO25,50
            //   ^ FB250,4,,
            //^ FDFD command that IS\&
            // preceded by an FB \&command.
            // ^ FS
            // ^ XZ
            List<string> result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add("^FB" + context.Scale(Width) + "," + MaxLineCount + "," + context.Scale(LineSpace) + "," + TextJustification + "," + context.Scale(HangingIndent));
            result.Add(RenderFieldDataSection());

            return result;
        }
    }

    //Similar to ZPLTextField with big line spacing, so only the first line is visible
    public class ZPLSingleLineFieldBlock : ZPLFieldBlock
    {
        public ZPLSingleLineFieldBlock(string text, int positionX, int positionY, int width, ZPLFont font, string textJustification = "L", NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true, bool reversePrint = false)
            : base(text, positionX, positionY, width, font, 9999, 9999, textJustification, 0, newLineConversion, useHexadecimalIndicator, reversePrint)
        {
        }
    }

    //The ^TB command prints a text block with defined width and height. The text block has an automatic word-wrap function.If the text exceeds the block height, the text is truncated. Does not support \n
    public class ZPLTextBlock : ZPLTextField
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public ZPLTextBlock(string text, int positionX, int positionY, int width, int height, ZPLFont font, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true)
            : base(text, positionX, positionY, font, newLineConversion, useHexadecimalIndicator)
        {
            Width = width;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add("^TB" + Font.Orientation + "," + context.Scale(Width) + "," + context.Scale(Height));
            result.Add(RenderFieldDataSection());

            return result;
        }
    }

    public class ZPLQRCode : ZPLPositionedElementBase
    {
        public string Content { get; protected set; }

        public int Model { get; set; }

        public int MagnificationFactor { get; set; }

        public string ErrorCorrection { get; set; }
        public int MaskValue { get; set; }

        public ZPLQRCode(string content, int positionX, int positionY, int model = 2, int magnificationFactor = 2, string errorCorrection = "Q", int maskValue = 7) : base(positionX, positionY)
        {
            Content = content;
            Model = model;
            MagnificationFactor = magnificationFactor;
            ErrorCorrection = errorCorrection;
            MaskValue = maskValue;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^ FO100,100
            //^ BQN,2,10
            //^ FDMM,AAC - 42 ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^BQN," + Model + "," + context.Scale(MagnificationFactor) + "," + ErrorCorrection + "," + MaskValue);
            result.Add("^FD" + ErrorCorrection + "M," + Content + "^FS");

            return result;
        }
    }

    public class ZPLReferenceGrid : ZPLElementBase
    {
        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();
            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZPLGraphicBox(0, i, 3000, 1).Render());
            }
            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZPLGraphicBox(i, 0, 1, 3000).Render());
            }
            return result;
        }
    }

    public class ZPLFontIdentifier : ZPLElementBase
    {
        public string FontReplaceLetter { get; set; }
        public string Device { get; set; }
        public string FontFileName { get; set; }

        public ZPLFontIdentifier(string fontReplaceLetter, string device, string fontFileName)
        {
            FontReplaceLetter = fontReplaceLetter;
            Device = device;
            FontFileName = fontFileName;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^CWa,d:o.x
            List<string> result = new List<string>();

            result.Add("^CW" + FontReplaceLetter + "," + Device + ":" + FontFileName);

            return result;
        }
    }
}
