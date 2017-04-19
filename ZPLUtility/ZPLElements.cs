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

        public abstract IEnumerable<string> Render(ZPLContext context);
    }

    public abstract class ZPLPositionedElementBase : ZPLElementBase
    {
        public ZPLOrigin Origin { get; protected set; }

        public ZPLPositionedElementBase(int positionX, int positionY) : base()
        {
            Origin = new ZPLOrigin(positionX, positionY);
        }
    }

    public abstract class ZPLGraphicElement : ZPLPositionedElementBase
    {
        //Line color
        public string LineColor { get; protected set; }

        public int BorderThickness { get; protected set; }

        public ZPLGraphicElement(int positionX, int positionY, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY)
        {
            BorderThickness = borderThickness;
            LineColor = lineColor;
        }
    }

    public class ZPLRaw : ZPLElementBase
    {
        public string RawContent { get; set; }

        public override IEnumerable<string> Render(ZPLContext context)
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

        public ZPLFont(int fontWidth = 30, int fontHeight = 30, string fontName = "0", string orientation = "")
        {
            FontName = fontName;
            Orientation = orientation;
            FontWidth = fontWidth;
            FontHeight = fontHeight;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            return new[] { "^A" + FontName + Orientation + "," + context.Scale(FontHeight) + "," + context.Scale(FontWidth) };
        }
    }

    public class ZPLBarCodeFieldDefault : ZPLElementBase
    {
        public int ModuleWidth { get; private set; }

        public double BarWidthRatio { get; private set; }

        public int Height { get; private set; }

        public ZPLBarCodeFieldDefault(int moduleWidth = 2, double barWidthRatio = 0.1d, int height = 10)
        {
            ModuleWidth = moduleWidth;
            BarWidthRatio = barWidthRatio;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLContext context)
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

        public override IEnumerable<string> Render(ZPLContext context)
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
        public ZPLTextField(string text, int positionX, int positionY, ZPLFont font, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true) : base(positionX, positionY)
        {
            Text = text;
            Origin = new ZPLOrigin(positionX, positionY);
            Font = font;
            UseHexadecimalIndicator = useHexadecimalIndicator;
            NewLineConversion = newLineConversion;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            List<string> result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add(RenderFDSection());

            return result;
        }

        protected string RenderFDSection()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((UseHexadecimalIndicator ? "^FH" : "") + "^FD");
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

        public ZPLFieldBlock(string text, int positionX, int positionY, int width, ZPLFont font, int maxLineCount = 1, int lineSpace = 0, string textJustification = "L", int hangingIndent = 0, NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToZPLNewLine, bool useHexadecimalIndicator = true)
            : base(text, positionX, positionY, font, newLineConversion, useHexadecimalIndicator)
        {
            TextJustification = textJustification;
            Width = width;
            MaxLineCount = maxLineCount;
            LineSpace = lineSpace;
            HangingIndent = hangingIndent;
        }

        public override IEnumerable<string> Render(ZPLContext context)
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
            result.Add(RenderFDSection());

            return result;
        }
    }

    //Similar to ZPLTextField with big line spacing, so only the first line is visible
    public class ZPLSingleLineFieldBlock : ZPLFieldBlock
    {
        public ZPLSingleLineFieldBlock(string text, int positionX, int positionY, int width, ZPLFont font, string textJustification = "L", NewLineConversionMethod newLineConversion = NewLineConversionMethod.ToSpace, bool useHexadecimalIndicator = true)
            : base(text, positionX, positionY, width, font, 9999, 9999, textJustification, 0, newLineConversion, useHexadecimalIndicator)
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

        public override IEnumerable<string> Render(ZPLContext context)
        {
            List<string> result = new List<string>();
            result.AddRange(Font.Render(context));
            result.AddRange(Origin.Render(context));
            result.Add("^TB" + Font.Orientation + "," + context.Scale(Width) + "," + context.Scale(Height));
            result.Add(RenderFDSection());

            return result;
        }
    }

    public abstract class ZPLBarcode : ZPLPositionedElementBase
    {
        public ZPLBarcode(int positionX, int positionY) : base(positionX, positionY)
        {
        }

        public int Height { get; protected set; }

        public string Orientation { get; protected set; }

        public string Content { get; protected set; }
    }

    public class ZPLBarCode128 : ZPLBarcode
    {
        public bool PrintInterpretationLine { get; private set; }
        public bool PrintInterpretationLineAboveCode { get; private set; }

        public ZPLBarCode128(string content, int positionX, int positionY, int height = 100, string orientation = "", bool printInterpretationLine = true, bool printInterpretationLineAboveCode = false) : base(positionX, positionY)
        {
            Content = content;
            Origin = new ZPLOrigin(positionX, positionY);
            Orientation = orientation;
            Height = height;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        public override IEnumerable<string> Render(ZPLContext context)
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

    public class ZPLGraphicBox : ZPLGraphicElement
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        //0~8
        public int CornerRounding { get; private set; }

        public ZPLGraphicBox(int positionX, int positionY, int width, int height, int borderThickness = 1, string lineColor = "B", int cornerRounding = 0) : base(positionX, positionY, borderThickness, lineColor)
        {
            Width = width;
            Height = height;

            CornerRounding = cornerRounding;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            //^ FO50,50
            //^ GB300,200,10 ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^GB" + context.Scale(Width) + "," + context.Scale(Height) + "," + context.Scale(BorderThickness) + "," + LineColor + "," + context.Scale(CornerRounding) + "^FS");

            return result;
        }
    }

    public class ZPLGraphicDiagonalLine : ZPLGraphicBox
    {
        public bool RightLeaningiagonal { get; private set; }

        public ZPLGraphicDiagonalLine(int positionX, int positionY, int width, int height, int borderThickness = 1, bool rightLeaningiagonal = false, string lineColor = "B", int cornerRounding = 0) : base(positionX, positionY, width, height, borderThickness, lineColor, 0)
        {
            RightLeaningiagonal = rightLeaningiagonal;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            //^GDw,h,t,c,o
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^GD" + context.Scale(Width) + "," + context.Scale(Height) + "," + context.Scale(BorderThickness) + "," + LineColor + "," + (RightLeaningiagonal ? "R" : "L") + "^FS");

            return result;
        }
    }

    public class ZPLGraphicEllipse : ZPLGraphicBox
    {
        public ZPLGraphicEllipse(int positionX, int positionY, int width, int height, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY, width, height, borderThickness, lineColor, 0)
        {
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            //^ GE300,100,10,B ^ FS
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^GE" + context.Scale(Width) + "," + context.Scale(Height) + "," + context.Scale(BorderThickness) + "," + LineColor + "^FS");

            return result;
        }
    }

    public class ZPLGraphicCircle : ZPLGraphicElement
    {
        public int Diameter { get; private set; }

        public ZPLGraphicCircle(int positionX, int positionY, int diameter, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY, borderThickness, lineColor)
        {
            Diameter = diameter;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            //^GCd,t,c
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^GC" + context.Scale(Diameter) + "," + context.Scale(BorderThickness) + "," + LineColor + "^FS");

            return result;
        }
    }

    public class ZPLGraphicSymbol : ZPLPositionedElementBase
    {
        public string Orientation { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public GraphicSymbolCharacter Character { get; private set; }

        string CharacterLetter
        {
            get
            {
                switch (Character)
                {
                    case GraphicSymbolCharacter.RegisteredTradeMark:
                        return "A";
                    case GraphicSymbolCharacter.Copyright:
                        return "B";
                    case GraphicSymbolCharacter.TradeMark:
                        return "C";
                    case GraphicSymbolCharacter.UnderwritersLaboratoriesApproval:
                        return "D";
                    case GraphicSymbolCharacter.CanadianStandardsAssociationApproval:
                        return "E";
                    default:
                        return "";
                }
            }
        }

        public ZPLGraphicSymbol(GraphicSymbolCharacter character, int positionX, int positionY, int width, int height, string orientation = "N") : base(positionX, positionY)
        {
            Character = character;
            Orientation = orientation;
            Width = width;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLContext context)
        {
            //^GSo,h,w
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add("^GS" + Orientation + "," + context.Scale(Height) + "," + context.Scale(Width) + "^FD" + CharacterLetter + "^FS");

            return result;
        }

        public enum GraphicSymbolCharacter
        {
            RegisteredTradeMark,
            Copyright,
            TradeMark,
            UnderwritersLaboratoriesApproval,
            CanadianStandardsAssociationApproval
        }
    }
}
