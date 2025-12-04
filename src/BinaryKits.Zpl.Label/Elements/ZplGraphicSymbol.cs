using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicSymbol : ZplPositionedElementBase
    {
        public FieldOrientation FieldOrientation { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public GraphicSymbolCharacter? Character { get; private set; }

        public ZplGraphicSymbol(
            string text,
            int positionX,
            int positionY,
            int width,
            int height,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(positionX, positionY, bottomToTop, useDefaultPosition)
        {
            GraphicSymbolCharacter? character = default;
            if (!string.IsNullOrEmpty(text))
            {
                character = (GraphicSymbolCharacter)text[0];
            }

            Character = character;
            FieldOrientation = fieldOrientation;
            Width = width;
            Height = height;
        }

        public ZplGraphicSymbol(
            GraphicSymbolCharacter? character,
            int positionX,
            int positionY,
            int width,
            int height,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(positionX, positionY, bottomToTop, useDefaultPosition)
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
            result.Add($"^GS{RenderFieldOrientation(FieldOrientation)},{context.Scale(Height)},{context.Scale(Width)}^FD{(char?)Character}^FS");

            return result;
        }
    }
}
