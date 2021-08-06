namespace BinaryKits.Zpl.Viewer.Models
{
    public class LabelHome
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public LabelHome(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
