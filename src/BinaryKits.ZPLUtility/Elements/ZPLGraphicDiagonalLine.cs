using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
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
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^GD{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{LineColor},{(RightLeaningiagonal ? "R" : "L")}^FS");

            return result;
        }
    }
}
