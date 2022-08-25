using System;
using System.Globalization;
using static System.FormattableString;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Bar Code Field Default
    /// The ^BY command is used to change the default values for the module width (in dots), the wide bar to narrow 
    /// bar width ratio and the bar code height(in dots). It can be used as often as necessary within a label format.
    /// </summary>
    public class BarCodeFieldDefaultCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^BY";

        /// <summary>
        /// Module width
        /// </summary>
        public int ModuleWidth { get; private set; } = 2;

        /// <summary>
        /// Wide bar to narrow bar width ratio
        /// </summary>
        public double WideBarToNarrowBarWidthRatio { get; private set; } = 3.0;

        /// <summary>
        /// Bar code height
        /// </summary>
        public int BarCodeHeight { get; private set; } = 10;

        /// <summary>
        /// Bar Code Field Default
        /// </summary>
        public BarCodeFieldDefaultCommand()
        { }

        /// <summary>
        /// Bar Code Field Default
        /// </summary>
        /// <param name="moduleWidth">Module width (1 to 10)</param>
        /// <param name="wideBarToNarrowBarWidthRatio">Wide bar to narrow bar width ratio (2.0 to 3.0, in 0.1 increments)</param>
        /// <param name="barCodeHeight">Bar code height</param>
        public BarCodeFieldDefaultCommand(
            int moduleWidth,
            double wideBarToNarrowBarWidthRatio,
            int barCodeHeight)
            : this()
        {
            if (ValidateIntParameter(nameof(moduleWidth), moduleWidth, 1, 10))
            {
                this.ModuleWidth = moduleWidth;
            }

            this.WideBarToNarrowBarWidthRatio = wideBarToNarrowBarWidthRatio;
            this.BarCodeHeight = barCodeHeight;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return Invariant($"{CommandPrefix}{this.ModuleWidth},{this.WideBarToNarrowBarWidthRatio:0.0},{this.BarCodeHeight}");
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new BarCodeFieldDefaultCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var moduleWidth))
                {
                    command.ModuleWidth = moduleWidth;
                }
            }

            if (zplDataParts.Length > 1)
            {
                if (double.TryParse(zplDataParts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var wideBarToNarrowBarWidthRatio))
                {
                    command.WideBarToNarrowBarWidthRatio = wideBarToNarrowBarWidthRatio;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var barCodeHeight))
                {
                    command.BarCodeHeight = barCodeHeight;
                }
            }

            return command;
        }

    }
}
