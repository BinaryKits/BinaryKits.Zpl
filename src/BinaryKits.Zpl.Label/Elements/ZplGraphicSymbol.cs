using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicSymbol : ZplPositionedElementBase
    {
        public FieldOrientation FieldOrientation { get; private set; }
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

        public ZplGraphicSymbol(
            GraphicSymbolCharacter character,
            int positionX,
            int positionY,
            int width,
            int height,
            FieldOrientation fieldOrientation = FieldOrientation.Normal)
            : base(positionX, positionY)
        {
            Character = character;
            FieldOrientation = fieldOrientation;
            Width = width;
            Height = height;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^GSo,h,w
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^GS{RenderFieldOrientation(FieldOrientation)},{context.Scale(Height)},{context.Scale(Width)}^FD{CharacterLetter}^FS");

            return result;
        }
    }
}
