namespace BinaryKits.ZplUtility.Elements
{
    public abstract class ZplGraphicElement : ZplPositionedElementBase
    {
        /// <summary>
        /// Line color
        /// </summary>
        public string LineColor { get; protected set; }

        public int BorderThickness { get; protected set; }

        public ZplGraphicElement(int positionX, int positionY, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY)
        {
            BorderThickness = borderThickness;
            LineColor = lineColor;
        }
    }
}
