namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplBarcode : ZplPositionedElementBase
    {
        public ZplBarcode(
            string content,
            int positionX,
            int positionY,
            int height,
            FieldOrientation fieldOrientation,
            bool printInterpretationLine,
            bool printInterpretationLineAboveCode) 
            : base(positionX, positionY)
        {
            Origin = new ZplOrigin(positionX, positionY);
            Content = content;
            Height = height;
            FieldOrientation = fieldOrientation;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

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
    }
}
