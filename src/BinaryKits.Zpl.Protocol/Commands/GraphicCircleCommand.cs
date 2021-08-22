namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Graphic Circle<br/>
    /// The ^GC command produces a circle on the printed label. The command parameters specify the diameter
    /// (width) of the circle, outline thickness, and color. Thickness extends inward from the outline.
    /// </summary>
    public class GraphicCircleCommand : CommandBase
    {
        /// <summary>
        /// Circle diameter
        /// </summary>
        public int CircleDiameter { get; private set; } = 3;

        /// <summary>
        /// Border thickness
        /// </summary>
        public int BorderThickness { get; private set; } = 1;

        /// <summary>
        /// Line color
        /// </summary>
        public LineColor LineColor { get; private set; } = LineColor.Black;

        /// <summary>
        /// Graphic Circle
        /// </summary>
        public GraphicCircleCommand() : base("^GC")
        { }

        /// <summary>
        /// Graphic Circle
        /// </summary>
        /// <param name="circleDiameter">Circle diameter (3 to 4095)</param>
        /// <param name="borderThickness">Border thickness (1 to 4095)</param>
        /// <param name="lineColor">Line color</param>
        public GraphicCircleCommand(
            int? circleDiameter = null,
            int? borderThickness = null,
            LineColor lineColor = LineColor.Black)
            : this()
        {
            if (this.ValidateIntParameter(nameof(circleDiameter), circleDiameter, 3, 4095))
            {
                this.CircleDiameter = circleDiameter.Value;
            }

            if (this.ValidateIntParameter(nameof(borderThickness), borderThickness, 1, 4095))
            {
                this.BorderThickness = borderThickness.Value;
            }

            this.LineColor = lineColor;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.CircleDiameter},{this.BorderThickness},{this.RenderLineColor(this.LineColor)}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var circleDiameter))
                {
                    this.CircleDiameter = circleDiameter;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var borderThickness))
                {
                    this.BorderThickness = borderThickness;
                }
            }

            if (zplDataParts.Length > 2)
            {
                var lineColorTemp = zplDataParts[2];
                this.LineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }
        }
    }
}
