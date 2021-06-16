using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
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
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^GB{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{LineColor},{context.Scale(CornerRounding)}^FS");

            return result;
        }
    }
}
