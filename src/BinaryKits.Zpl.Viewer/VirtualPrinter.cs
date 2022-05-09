using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Viewer.Models;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Viewer
{
    public class VirtualPrinter
    {
        public LabelHome LabelHomePosition { get; private set; }
        public LabelPosition NextElementPosition { get; private set; }
        public FieldDataBase NextElementFieldData { get; private set; }
        public FieldBlock NextElementFieldBlock { get; private set; }
        public int FontWidth { get; private set; } = 0;
        public int FontHeight { get; private set; } = 10;
        public string FontName { get; private set; } = "0";
        public List<string> Comments { get; private set; }

        /// <summary>
        /// Override the default font, only for the next element
        /// </summary>
        public FontInfo NextFont { get; private set; }

        public bool NextElementFieldReverse { get; private set; }
        public bool LabelReverse { get; private set; }
        public BarcodeInfo BarcodeInfo { get; private set; }

        public string NextDownloadFormatName { get; private set; }
        public int? NextFieldNumber { get; private set; }

        public VirtualPrinter()
        {
            this.BarcodeInfo = new BarcodeInfo();
            this.Comments = new List<string>();
        }

        public void SetNextElementPosition(int x, int y, bool calculateFromBottom = false)
        {
            this.NextElementPosition = new LabelPosition(x, y, calculateFromBottom);
        }

        public void ClearNextElementPosition()
        {
            this.NextElementPosition = new LabelPosition(0, 0, false);
        }

        public void SetNextElementFieldData(FieldDataBase fieldData)
        {
            this.NextElementFieldData = fieldData;
        }

        public void ClearNextElementFieldData()
        {
            this.NextElementFieldData = null;
        }

        public void SetNextElementFieldBlock(FieldBlock fieldBlock)
        {
            this.NextElementFieldBlock = fieldBlock;
        }

        public void ClearNextElementFieldBlock()
        {
            this.NextElementFieldBlock = null;
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

        public void SetNextElementFieldReverse()
        {
            this.NextElementFieldReverse = true;
        }

        public void SetLabelReverse(bool reverse)
        {
            this.LabelReverse = reverse;
        }

        public void ClearNextElementFieldReverse()
        {
            this.NextElementFieldReverse = false;
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

        public void SetLabelHome(int x, int y)
        {
            this.LabelHomePosition = new LabelHome(x, y);
        }

        public void AddComment(string comment)
        {
            this.Comments.Add(comment);
        }

        public void ClearComments()
        {
            this.Comments.Clear();
        }

        public void SetNextDownloadFormatName(string name)
        {
            this.NextDownloadFormatName = name;
        }

        public void ClearNextDownloadFormatName()
        {
            this.NextDownloadFormatName = null;
        }

        public void SetNextFieldNumber(int number)
        {
            this.NextFieldNumber = number;
        }

        public void ClearNextFieldNumber()
        {
            this.NextFieldNumber = null;
        }
    }
}
