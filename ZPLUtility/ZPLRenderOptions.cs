using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
    /// <summary>
    /// Holding redering settings
    /// </summary>
    public class ZPLRenderOptions
    {
        //Include ^XA and ^XZ
        public bool AddStartEndFormat { get; set; }
        //Include ^LH0,0
        public bool AddDefaultLabelHome { get; set; }

        //^CI
        public string ChangeInternationalFontEncoding { get; set; }

        public string DefaultTextOrientation { get; set; }

        public bool DisplayComments { get; set; }

        public bool AddEmptyLineBeforeElementStart { get; set; }

        public int SourcePrintDPI { get; set; }

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
