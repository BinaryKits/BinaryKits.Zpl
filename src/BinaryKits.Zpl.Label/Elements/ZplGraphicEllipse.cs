using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplGraphicEllipse : ZplGraphicBox
    {
        public ZplGraphicEllipse(
            int positionX,
            int positionY,
            int width,
            int height,
            int borderThickness = 1,
            LineColor lineColor = LineColor.Black)
            : base(positionX, positionY, width, height, borderThickness, lineColor, 0)
        {
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
