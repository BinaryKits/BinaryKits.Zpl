using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicBox : ZplGraphicElement
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        //0~8
        public int CornerRounding { get; private set; }

        public ZplGraphicBox(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            LineColor lineColor = LineColor.Black,
            int cornerRounding = 0,
            bool reversePrint = false,
            bool bottomToTop = false)
            : base(positionX, positionY, borderThickness, lineColor, reversePrint, bottomToTop)
        {
            Width = width;
            Height = height;

            CornerRounding = cornerRounding;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO50,50
            //^ GB300,200,10 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^GB{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{RenderLineColor(LineColor)},{context.Scale(CornerRounding)}^FS");

            return result;
        }
    }
}
