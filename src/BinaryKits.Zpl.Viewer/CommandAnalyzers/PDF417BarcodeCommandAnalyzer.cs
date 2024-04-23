using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;
using System;
using System.Globalization;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class PDF417ZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public PDF417ZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^B7", virtualPrinter) { }
        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            // reusable buffer
            int tmpint;

            /*
             * Format: ^B7o,h,s,c,r,t, parse order:
             * orientation
             * height
             * security level
             * columns
             * rows
             * compact
            */

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            
            int height = this.VirtualPrinter.BarcodeInfo.Height;
            if (zplDataParts.Length > 1 && int.TryParse(zplDataParts[1], out tmpint))
            {
                height = tmpint;
            }
            else if (zplDataParts.Length > 1)
            {
                //Sometimes a decimal is given, this works around that.
                var tempDecimal = Convert.ToDecimal(zplDataParts[1], new CultureInfo("en-US"));
                var tempHeight = (int)Math.Floor(tempDecimal);
                if (tempHeight > 0)
                {
                    height = tempHeight;
                }
            }

            int securityLevel = 0;
            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out tmpint))
            {
                securityLevel = tmpint;
            }

            int? columns = null;
            if (zplDataParts.Length > 3 && int.TryParse(zplDataParts[3], out tmpint))
            {
                columns = tmpint;
            }

            int? rows = null;
            if (zplDataParts.Length > 4 && int.TryParse(zplDataParts[4], out tmpint))
            {
                rows = tmpint;
            }
            
            bool compact = false;
            if (zplDataParts.Length > 5)
            {
                compact = ConvertBoolean(zplDataParts[5]);
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            this.VirtualPrinter.SetNextElementFieldData(new PDF417FieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                SecurityLevel = securityLevel,
                Columns = columns,
                Rows = rows,
                Compact = compact
            });

            return null;
        }
    }
}
