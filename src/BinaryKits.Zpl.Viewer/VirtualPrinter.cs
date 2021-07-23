using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer
{
    public class VirtualPrinter
    {
        public ElementPosition NextElementPosition { get; private set; }
        public FieldDataBase NextFieldDataElement { get; private set; }
        public int FontWidth { get; private set; } = 0;
        public int FontHeight { get; private set; } = 10;
        public string FontName { get; private set; } = "0";

        /// <summary>
        /// Override the default font, only for the next element
        /// </summary>
        public FontInfo NextFont { get; private set; }

        public BarcodeInfo BarcodeInfo { get; private set; }

        public VirtualPrinter()
        {
            this.BarcodeInfo = new BarcodeInfo();
        }

        public void SetNextElementPosition(int x, int y)
        {
            this.NextElementPosition = new ElementPosition(x, y);
        }

        public void ClearNextElementPosition()
        {
            this.NextElementPosition = new ElementPosition(0, 0);
        }

        public void SetNextFieldDataElement(FieldDataBase fieldData)
        {
            this.NextFieldDataElement = fieldData;
        }

        public void ClearNextFieldDataElement()
        {
            this.NextFieldDataElement = null;
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

        public void SetBarcodeModuleWidth(int moduleWidth)
        {
            this.BarcodeInfo.ModuleWidth = moduleWidth;
        }

        public void SetBarcodeWideBarToNarrowBarWidthRatio(double wideBarToNarrowBarWidthRatio)
        {
            this.BarcodeInfo.WideBarToNarrowBarWidthRatio = wideBarToNarrowBarWidthRatio;
        }

        public void SetBarcodeHeight(int height)
        {
            this.BarcodeInfo.Height = height;
        }
    }
}
