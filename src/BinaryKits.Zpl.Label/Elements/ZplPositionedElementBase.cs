namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplPositionedElementBase : ZplElementBase
    {
        public ZplOrigin Origin { get; protected set; }

        public ZplPositionedElementBase(int positionX, int positionY) : base()
        {
            Origin = new ZplOrigin(positionX, positionY);
        }
    }
}
