namespace BinaryKits.Utility.ZPLUtility.Elements
{
    public abstract class ZPLPositionedElementBase : ZPLElementBase
    {
        public ZPLOrigin Origin { get; protected set; }

        public ZPLPositionedElementBase(int positionX, int positionY) : base()
        {
            Origin = new ZPLOrigin(positionX, positionY);
        }
    }
}
