namespace BinaryKits.Zpl.Viewer
{
    public class LabelPosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public LabelPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
