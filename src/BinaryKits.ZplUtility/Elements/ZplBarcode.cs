namespace BinaryKits.ZplUtility.Elements
{
    public abstract class ZplBarcode : ZplPositionedElementBase
    {
        public ZplBarcode(string content, int positionX, int positionY, int height, string orientation, bool printInterpretationLine, bool printInterpretationLineAboveCode) : base(positionX, positionY)
        {
            Origin = new ZplOrigin(positionX, positionY);
            Content = content;
            Height = height;
            Orientation = orientation;
            PrintInterpretationLine = printInterpretationLine;
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode;
        }

        public int Height { get; protected set; }

        public string Orientation { get; protected set; }

        public string Content { get; protected set; }
        public bool PrintInterpretationLine { get; protected set; }
        public bool PrintInterpretationLineAboveCode { get; protected set; }

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
