namespace BinaryKits.Utility.ZPLUtility
{
    public abstract class ZPLGraphicElement : ZPLPositionedElementBase
    {
        /// <summary>
        /// Line color
        /// </summary>
        public string LineColor { get; protected set; }

        public int BorderThickness { get; protected set; }

        public ZPLGraphicElement(int positionX, int positionY, int borderThickness = 1, string lineColor = "B") : base(positionX, positionY)
        {
            BorderThickness = borderThickness;
            LineColor = lineColor;
        }
    }
}
