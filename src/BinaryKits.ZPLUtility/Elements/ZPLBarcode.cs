namespace BinaryKits.Utility.ZPLUtility.Elements
{
    public abstract class ZPLBarcode : ZPLPositionedElementBase
    {
        public ZPLBarcode(string content, int positionX, int positionY, int height, string orientation, bool printInterpretationLine, bool printInterpretationLineAboveCode) : base(positionX, positionY)
        {
            Origin = new ZPLOrigin(positionX, positionY);
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
    }
}
