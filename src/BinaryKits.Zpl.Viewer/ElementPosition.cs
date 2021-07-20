namespace BinaryKits.Zpl.Viewer
{
    public class ElementPosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public ElementPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
