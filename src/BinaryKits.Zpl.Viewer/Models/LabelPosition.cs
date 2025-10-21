namespace BinaryKits.Zpl.Viewer.Models
{
    public class LabelPosition
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool CalculateFromBottom { get; private set; }
        public bool UseDefaultPosition { get; private set; }

        public LabelPosition(int x, int y, bool calculateFromBottom, bool useDefaultPosition)
        {
            this.X = x;
            this.Y = y;
            this.CalculateFromBottom = calculateFromBottom;
            this.UseDefaultPosition = useDefaultPosition;
        }
    }
}
