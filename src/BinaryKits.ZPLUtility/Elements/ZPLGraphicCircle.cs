using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    public class ZPLGraphicCircle : ZPLGraphicElement
    {
        public int Diameter { get; private set; }

        public ZPLGraphicCircle(int positionX, int positionY, int diameter, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY, borderThickness, lineColor)
        {
            Diameter = diameter;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^GCd,t,c
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^GC{context.Scale(Diameter)},{context.Scale(BorderThickness)},{LineColor}^FS");

            return result;
        }
    }
}
