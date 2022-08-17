using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicDiagonalLine : ZplGraphicBox
    {
        public bool RightLeaningDiagonal { get; private set; }

        public ZplGraphicDiagonalLine(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            bool rightLeaningDiagonal = false,
            LineColor lineColor = LineColor.Black)
            : base(positionX, positionY, width, height, borderThickness, lineColor, 0)
        {
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
