using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicDiagonalLine : ZplGraphicElement
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool RightLeaningDiagonal { get; private set; }

        public ZplGraphicDiagonalLine(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            LineColor lineColor = LineColor.Black,
            bool rightLeaningDiagonal = true,
            bool reversePrint = false,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(positionX, positionY, borderThickness, lineColor, reversePrint, bottomToTop, useDefaultPosition)
        {
            Width = width;
            Height = height;
            
            RightLeaningDiagonal = rightLeaningDiagonal;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^GDw,h,t,c,o
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^GD{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{RenderLineColor(LineColor)},{(RightLeaningDiagonal ? "R" : "L")}^FS");

            return result;
        }
    }
}
