using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
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
        public FieldOrientation FieldOrientation { get; private set; }
        public int FontWidth { get; private set; }
        public int FontHeight { get; private set; }

        public ZplFont(
            int fontWidth = 30,
            int fontHeight = 30,
            string fontName = "0",
            FieldOrientation fieldOrientation = FieldOrientation.Normal)
        {
            FontName = fontName;
            FieldOrientation = fieldOrientation;
            FontWidth = fontWidth;
            FontHeight = fontHeight;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            return new[] { $"^A{FontName}{RenderFieldOrientation(FieldOrientation)},{context.Scale(FontHeight)},{context.Scale(FontWidth)}" };
        }
    }
}
