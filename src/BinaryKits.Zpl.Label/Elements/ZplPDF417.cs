using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// PDF417 Barcode ^B7o,h,s,c,r,t
    /// </summary>
    public class ZplPDF417 : ZplFieldDataElementBase
    {
        public int Height { get; protected set; }
        public int ModuleWidth { get; protected set; }
        public int? Columns { get; protected set; }
        public int? Rows { get; protected set; }
        public bool Compact { get; protected set; }
        public int SecurityLevel { get; protected set; }

        /// <summary>
        /// Zpl PDF417 barcode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="moduleWidth"></param>
        /// <param name="columns">1-30: Number of data columns to encode. Default will auto balance 1:2 row to column</param>
        /// <param name="rows">3-90. Number of data columns to encode. Default will auto balance 1:2 row to column</param>
        /// <param name="compact">Truncate right row indicators and stop pattern</param>
        /// <param name="securityLevel">1-8 This determines the number of error detection and correction code-words to be generated for the symbol.The default level (0) provides only error detection without correction.Increasing the security level adds increasing levels of error correction and increases the symbol size.</param>
        /// <param name="fieldOrientation"></param>
        /// <param name="hexadecimalIndicator"></param>
        /// <param name="bottomToTop"></param>
        /// <param name="useDefaultPosition"></param>
        public ZplPDF417(
            string content,
            int positionX,
            int positionY,
            int height = 8,
            int moduleWidth = 2,
            int? columns = null,
            int? rows = null,
            bool compact = false,
            int securityLevel = 0,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            char? hexadecimalIndicator = null,
            bool bottomToTop = false,
            bool useDefaultPosition = false)
            : base(content, positionX, positionY, fieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition)
        {
            Height = height;
            ModuleWidth = moduleWidth;
            Columns = columns;
            Rows = rows;
            Compact = compact;
            SecurityLevel = securityLevel;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO100,100
            //^ BQN,2,10
            //^ FDMM,AAC - 42 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BX{RenderFieldOrientation()},{context.Scale(Height)}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
