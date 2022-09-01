namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplBarcode : ZplPositionedElementBase, IFormatElement
    {
        public ZplBarcode(
            string content,
            int positionX,
            int positionY,
            int height,
            int moduleWidth,
            double wideBarToNarrowBarWidthRatio,
            FieldOrientation fieldOrientation,
            bool printInterpretationLine,
            bool printInterpretationLineAboveCode,
            bool bottomToTop = false)
            : base(positionX, positionY, bottomToTop)
        {
            Content = content;
            Height = height;
            ModuleWidth = moduleWidth;
            WideBarToNarrowBarWidthRatio = wideBarToNarrowBarWidthRatio;
            FieldOrientation = fieldOrientation;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        /// <summary>
        /// Module width (in dots)
        /// </summary>
        public int ModuleWidth { get; protected set; }
        public double WideBarToNarrowBarWidthRatio { get; protected set; }

        public int Height { get; protected set; }

        public FieldOrientation FieldOrientation { get; protected set; }

        public string Content { get; protected set; }
        public bool PrintInterpretationLine { get; protected set; }
        public bool PrintInterpretationLineAboveCode { get; protected set; }

        public string RenderPrintInterpretationLine()
        {
            return PrintInterpretationLine ? "Y" : "N";
        }

        public string RenderPrintInterpretationLineAboveCode()
        {
            return PrintInterpretationLineAboveCode ? "Y" : "N";
        }

        protected string RenderFieldOrientation()
        {
            return RenderFieldOrientation(FieldOrientation);
        }

        protected string RenderModuleWidth()
        {
            return $"^BY{ModuleWidth}";
        }

        protected bool IsDigitsOnly(string text)
        {
            foreach (char c in text)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public void SetTemplateContent(string content)
        {
            Content = content;
        }
    }
}
