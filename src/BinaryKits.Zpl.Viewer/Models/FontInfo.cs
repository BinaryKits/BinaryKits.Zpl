using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer.Models
{
    public class FontInfo
    {
        public string FontName { get; private set; }
        public FieldOrientation FieldOrientation { get; private set; }
        public int FontWidth { get; private set; }
        public int FontHeight { get; private set; }

        public FontInfo(string fontName, FieldOrientation fieldOrientation, int fontWidth, int fontHeight)
        {
            this.FontName = fontName;
            this.FieldOrientation = fieldOrientation;
            this.FontWidth = fontWidth;
            this.FontHeight = fontHeight;
        }
    }
}
