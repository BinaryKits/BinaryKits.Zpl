using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplGraphicBox : ZplGraphicElement
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        //0~8
        public int CornerRounding { get; private set; }

        public ZplGraphicBox(int positionX, int positionY, int width, int height, int borderThickness = 1, string lineColor = "B", int cornerRounding = 0) : base(positionX, positionY, borderThickness, lineColor)
        {
            Width = width;
            Height = height;

            CornerRounding = cornerRounding;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
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
