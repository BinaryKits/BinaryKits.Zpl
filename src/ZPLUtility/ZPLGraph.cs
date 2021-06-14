using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
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

        public override IEnumerable<string> Render(ZPLRenderOptions context)
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

        public override IEnumerable<string> Render(ZPLRenderOptions context)
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

        public override IEnumerable<string> Render(ZPLRenderOptions context)
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

        public override IEnumerable<string> Render(ZPLRenderOptions context)
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

        public override IEnumerable<string> Render(ZPLRenderOptions context)
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
