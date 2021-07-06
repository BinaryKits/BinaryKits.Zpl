using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplGraphicDiagonalLine : ZplGraphicBox
    {
        public bool RightLeaningiagonal { get; private set; }

        public ZplGraphicDiagonalLine(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            bool rightLeaningiagonal = false,
            LineColor lineColor = LineColor.Black)
            : base(positionX, positionY, width, height, borderThickness, lineColor, 0)
        {
            RightLeaningiagonal = rightLeaningiagonal;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^GDw,h,t,c,o
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^GD{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{RenderLineColor(LineColor)},{(RightLeaningiagonal ? "R" : "L")}^FS");

            return result;
        }
    }
}
