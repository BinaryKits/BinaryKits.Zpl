namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Graphic Box<br/>
    /// The ^GB command is used to draw boxes and lines as part of a label format. Boxes and lines are used to
    /// highlight important information, divide labels into distinct areas, or to improve the appearance of a label.
    /// The same format command is used for drawing either boxes or lines.
    /// </summary>
    public class GraphicBoxCommand : CommandBase
    {
        /// <summary>
        /// Box width
        /// </summary>
        public int BoxWidth { get; private set; } = 1;

        /// <summary>
        /// Box height
        /// </summary>
        public int BoxHeight { get; private set; } = 1;

        /// <summary>
        /// Border thickness
        /// </summary>
        public int BorderThickness { get; private set; } = 1;

        /// <summary>
        /// Line color
        /// </summary>
        public LineColor LineColor { get; private set; } = LineColor.Black;

        /// <summary>
        /// Degree of cornerrounding
        /// </summary>
        public int DegreeOfCornerrounding { get; private set; } = 0;

        /// <summary>
        /// Graphic Box
        /// </summary>
        public GraphicBoxCommand() : base("^GB")
        { }

        /// <summary>
        /// Graphic Box
        /// </summary>
        /// <param name="boxWidth">Box width (borderThickness or 1 to 32000)</param>
        /// <param name="boxHeight">Box height (borderThickness or 1 to 32000)</param>
        /// <param name="borderThickness">Border thickness (1 to 32000)</param>
        /// <param name="lineColor">Line color</param>
        /// <param name="degreeOfCornerrounding">Degree of cornerrounding (0 to 8)</param>
        public GraphicBoxCommand(
            int? boxWidth = null,
            int? boxHeight = null,
            int? borderThickness = null,
            LineColor lineColor = LineColor.Black,
            int? degreeOfCornerrounding = null)
            : this()
        {
            if (this.ValidateIntParameter(nameof(borderThickness), borderThickness, 1, 32000))
            {
                this.BorderThickness = borderThickness.Value;
                this.BoxWidth = this.BorderThickness;
                this.BoxHeight = this.BorderThickness;
            }

            if (this.ValidateIntParameter(nameof(boxWidth), boxWidth, this.BorderThickness, 32000))
            {
                this.BoxWidth = boxWidth.Value;
            }

            if (this.ValidateIntParameter(nameof(boxHeight), boxHeight, this.BorderThickness, 32000))
            {
                this.BoxHeight = boxHeight.Value;
            }

            this.LineColor = lineColor;

            if (this.ValidateIntParameter(nameof(degreeOfCornerrounding), degreeOfCornerrounding, 0, 8))
            {
                this.DegreeOfCornerrounding = degreeOfCornerrounding.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.BoxWidth},{this.BoxHeight},{this.BorderThickness},{this.RenderLineColor(this.LineColor)},{this.DegreeOfCornerrounding}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var boxWidth))
                {
                    this.BoxWidth = boxWidth;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var boxHeight))
                {
                    this.BoxHeight = boxHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var borderThickness))
                {
                    this.BorderThickness = borderThickness;
                    if (this.BoxWidth < borderThickness)
                    {
                        this.BoxWidth = borderThickness;
                    }
                    if (this.BoxHeight < borderThickness)
                    {
                        this.BoxHeight = borderThickness;
                    }
                }
            }

            if (zplDataParts.Length > 3)
            {
                var lineColorTemp = zplDataParts[3];
                this.LineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var degreeOfCornerrounding))
                {
                    this.DegreeOfCornerrounding = degreeOfCornerrounding;
                }
            }
        }
    }
}
