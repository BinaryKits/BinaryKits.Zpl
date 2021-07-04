using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// ^A – Scalable/Bitmapped Font
    /// </summary>
    public class ZplFont : ZplElementBase
    {
        /// <summary>
        /// Any font in the printer (downloaded, EPROM, stored fonts, fonts A through Z and 0 to 9).
        /// </summary>
        public string FontName { get; private set; }
        public string Orientation { get; private set; }
        public int FontWidth { get; private set; }
        public int FontHeight { get; private set; }

        public ZplFont(int fontWidth = 30, int fontHeight = 30, string fontName = "0", string orientation = "N")
        {
            FontName = fontName;
            Orientation = orientation;
            FontWidth = fontWidth;
            FontHeight = fontHeight;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
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
