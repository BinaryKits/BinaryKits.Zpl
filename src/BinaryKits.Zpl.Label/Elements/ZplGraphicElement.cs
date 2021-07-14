namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplGraphicElement : ZplPositionedElementBase
    {
        /// <summary>
        /// Line color
        /// </summary>
        public LineColor LineColor { get; protected set; }

        public int BorderThickness { get; protected set; }

        public ZplGraphicElement(int positionX, int positionY, int borderThickness = 1, LineColor lineColor = LineColor.Black) : base(positionX, positionY)
        {
            BorderThickness = borderThickness;
            LineColor = lineColor;
        }
    }
}
