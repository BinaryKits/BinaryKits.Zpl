using System;
using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
    public class ZPLBarCodeFieldDefault : ZPLElementBase
    {
        public int ModuleWidth { get; private set; }

        public double BarWidthRatio { get; private set; }

        public int Height { get; private set; }

        public ZPLBarCodeFieldDefault(int moduleWidth = 2, double barWidthRatio = 3.0d, int height = 10)
        {
            ModuleWidth = moduleWidth;
            BarWidthRatio = barWidthRatio;
            Height = height;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            return new[] { $"^BY{context.Scale(ModuleWidth)},{Math.Round(BarWidthRatio, 1)},{context.Scale(Height)}" };
        }
    }
}
