using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicEllipse : ZplGraphicElement
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ZplGraphicEllipse(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            LineColor lineColor = LineColor.Black,
            bool reversePrint = false,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(positionX, positionY, borderThickness, lineColor, reversePrint, bottomToTop, useDefaultPosition)
        {
            Width = width;
            Height = height;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ GE300,100,10,B ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^GE{context.Scale(Width)},{context.Scale(Height)},{context.Scale(BorderThickness)},{RenderLineColor(LineColor)}^FS");

            return result;
        }
    }
}
