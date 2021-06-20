using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
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
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^GS{Orientation},{context.Scale(Height)},{context.Scale(Width)}^FD{CharacterLetter}^FS");

            return result;
        }
    }
}
