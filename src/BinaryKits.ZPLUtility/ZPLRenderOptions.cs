namespace BinaryKits.ZPLUtility
{
    /// <summary>
    /// Holding redering settings
    /// </summary>
    public class ZPLRenderOptions
    {
        /// <summary>
        /// Include ^XA and ^XZ
        /// </summary>
        public bool AddStartEndFormat { get; set; }
        /// <summary>
        /// Include ^LH0,0
        /// </summary>
        public bool AddDefaultLabelHome { get; set; }

        /// <summary>
        /// ^CI
        /// </summary>
        public string ChangeInternationalFontEncoding { get; set; }

        public string DefaultTextOrientation { get; set; }

        public bool DisplayComments { get; set; }

        public bool AddEmptyLineBeforeElementStart { get; set; }

        /// <summary>
        /// SourcePrint DPI
        /// </summary>
        /// <remarks>Default: 203</remarks>
        public int SourcePrintDPI { get; set; }

        /// <summary>
        /// TargetPrint DPI
        /// </summary>
        /// <remarks>Default: 203</remarks>
        public int TargetPrintDPI { get; set; }

        public bool CompressedRendering { get; set; }

        public double ScaleFactor { get { return (double)TargetPrintDPI / SourcePrintDPI; } }

        public int Scale(int input)
        {
            return (int)(input * ScaleFactor);
        }

        public double Scale(double input)
        {
            return input * ScaleFactor;
        }

        public ZPLRenderOptions()
        {
            AddStartEndFormat = true;
            AddDefaultLabelHome = true;
            ChangeInternationalFontEncoding = ZPLConstants.InternationalFontEncoding.CI28;
            SourcePrintDPI = TargetPrintDPI = 203;
            DefaultTextOrientation = "N";
        }
    }
}
