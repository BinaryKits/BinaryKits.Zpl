using System;

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
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^GB";

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
        public GraphicBoxCommand()
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
        {
            if (ValidateIntParameter(nameof(borderThickness), borderThickness, 1, 32000))
            {
                this.BorderThickness = borderThickness.Value;
                this.BoxWidth = this.BorderThickness;
                this.BoxHeight = this.BorderThickness;
            }

            if (ValidateIntParameter(nameof(boxWidth), boxWidth, this.BorderThickness, 32000))
            {
                this.BoxWidth = boxWidth.Value;
            }

            if (ValidateIntParameter(nameof(boxHeight), boxHeight, this.BorderThickness, 32000))
            {
                this.BoxHeight = boxHeight.Value;
            }

            this.LineColor = lineColor;

            if (ValidateIntParameter(nameof(degreeOfCornerrounding), degreeOfCornerrounding, 0, 8))
            {
                this.DegreeOfCornerrounding = degreeOfCornerrounding.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.BoxWidth},{this.BoxHeight},{this.BorderThickness},{RenderLineColor(this.LineColor)},{this.DegreeOfCornerrounding}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new GraphicBoxCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var boxWidth))
                {
                    command.BoxWidth = boxWidth;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var boxHeight))
                {
                    command.BoxHeight = boxHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var borderThickness))
                {
                    command.BorderThickness = borderThickness;
                    if (command.BoxWidth < borderThickness)
                    {
                        command.BoxWidth = borderThickness;
                    }
                    if (command.BoxHeight < borderThickness)
                    {
                        command.BoxHeight = borderThickness;
                    }
                }
            }

            if (zplDataParts.Length > 3)
            {
                var lineColorTemp = zplDataParts[3];
                command.LineColor = lineColorTemp == "W" ? LineColor.White : LineColor.Black;
            }

            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var degreeOfCornerrounding))
                {
                    command.DegreeOfCornerrounding = degreeOfCornerrounding;
                }
            }

            return command;
        }

    }
}
