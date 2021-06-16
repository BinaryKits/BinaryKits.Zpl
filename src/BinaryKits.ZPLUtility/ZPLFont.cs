using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
    //^A – Scalable/Bitmapped Font
    public class ZPLFont : ZPLElementBase
    {
        public string FontName { get; private set; }
        public string Orientation { get; private set; }
        public int FontWidth { get; private set; }
        public int FontHeight { get; private set; }

        public ZPLFont(int fontWidth = 30, int fontHeight = 30, string fontName = "0", string orientation = "N")
        {
            FontName = fontName;
            Orientation = orientation;
            FontWidth = fontWidth;
            FontHeight = fontHeight;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            string textOrientation = Orientation;
            if (string.IsNullOrEmpty(textOrientation))
            {
                textOrientation = context.DefaultTextOrientation;
            }
            return new[] { $"^A{FontName}{textOrientation},{context.Scale(FontHeight)},{context.Scale(FontWidth)}" };
        }
    }
}
