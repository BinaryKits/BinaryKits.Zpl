using BinaryKits.Zpl.Label;

namespace BinaryKits.Zpl.Viewer
{
    public class VirtualPrinter
    {
        public ElementPosition NextElementPosition { get; private set; }
        public int FontWidth { get; private set; } = 0;
        public int FontHeight { get; private set; } = 10;
        public string FontName { get; private set; } = "0";

        public FontInfo NextFont { get; private set; }

        public void SetNextElementPosition(int x, int y)
        {
            this.NextElementPosition = new ElementPosition(x, y);
        }

        public void ClearNextElementPosition()
        {
            this.NextElementPosition = new ElementPosition(0, 0);
        }

        public void SetNextFont(
            string fontName,
            FieldOrientation fieldOrientation,
            int fontWidth,
            int fontHeight)
        {
            this.NextFont = new FontInfo(fontName, fieldOrientation, fontWidth, fontHeight);
        }

        public void ClearNextFont()
        {
            this.NextFont = null;
        }

        public void SetFontWidth(int fontWidth)
        {
            this.FontWidth = fontWidth;
        }

        public void SetFontHeight(int fontHeight)
        {
            this.FontHeight = fontHeight;
        }

        public void SetFontName(string fontName)
        {
            this.FontName = fontName;
        }
    }
}
