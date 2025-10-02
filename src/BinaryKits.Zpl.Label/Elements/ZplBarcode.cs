using System.Text;

using static System.Net.Mime.MediaTypeNames;

namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplBarcode : ZplFieldDataElementBase
    {
        /// <summary>
        /// Module width (in dots)
        /// </summary>
        public int ModuleWidth { get; protected set; }
        public double WideBarToNarrowBarWidthRatio { get; protected set; }
        public int Height { get; protected set; }
        public bool PrintInterpretationLine { get; protected set; }
        public bool PrintInterpretationLineAboveCode { get; protected set; }

        public ZplBarcode(
            string content,
            int positionX,
            int positionY,
            int height,
            int moduleWidth,
            double wideBarToNarrowBarWidthRatio,
            FieldOrientation fieldOrientation,
            bool useHexadecimalIndicator,
            bool printInterpretationLine,
            bool printInterpretationLineAboveCode,
            bool bottomToTop)
            : base(content, positionX, positionY, fieldOrientation, useHexadecimalIndicator, bottomToTop)
        {
            Height = height;
            ModuleWidth = moduleWidth;
            WideBarToNarrowBarWidthRatio = wideBarToNarrowBarWidthRatio;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        protected string RenderPrintInterpretationLine()
        {
            return RenderBoolean(PrintInterpretationLine);
        }

        protected string RenderPrintInterpretationLineAboveCode()
        {
            return RenderBoolean(PrintInterpretationLineAboveCode);
        }

        protected string RenderModuleWidth()
        {
            return $"^BY{ModuleWidth},{WideBarToNarrowBarWidthRatio}";
        }

    }
}
