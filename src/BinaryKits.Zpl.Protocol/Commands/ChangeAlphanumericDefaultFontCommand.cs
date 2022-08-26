using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Change Alphanumeric Default Font<br/>
    /// The ^CF command sets the default font used in your printer. You can use the ^CF command to simplify your
    /// programs.
    /// </summary>
    public class ChangeAlphanumericDefaultFontCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^CF";

        /// <summary>
        /// Specified default font
        /// </summary>
        public char SpecifiedDefaultFont { get; private set; }

        /// <summary>
        /// Individual character height
        /// </summary>
        public int? IndividualCharacterHeight { get; private set; }

        /// <summary>
        /// Individual character width
        /// </summary>
        public int? IndividualCharacterWidth { get; private set; }

        /// <summary>
        /// Change Alphanumeric Default Font
        /// </summary>
        public ChangeAlphanumericDefaultFontCommand()
        { }

        /// <summary>
        /// Change Alphanumeric Default Font
        /// </summary>
        /// <param name="specifiedDefaultFont">Specified default font</param>
        /// <param name="individualCharacterHeight">Individual character height (0 to 32000)</param>
        /// <param name="individualCharacterWidth">Individual character width (0 to 32000)</param>
        public ChangeAlphanumericDefaultFontCommand(
            char specifiedDefaultFont,
            int? individualCharacterHeight = null,
            int? individualCharacterWidth = null)
        {
            this.SpecifiedDefaultFont = specifiedDefaultFont;

            if (ValidateIntParameter(nameof(individualCharacterHeight), individualCharacterHeight, 0, 32000))
            {
                this.IndividualCharacterHeight = individualCharacterHeight.Value;
            }

            if (ValidateIntParameter(nameof(individualCharacterWidth), individualCharacterWidth, 0, 32000))
            {
                this.IndividualCharacterWidth = individualCharacterWidth.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{CommandPrefix}{this.SpecifiedDefaultFont},{this.IndividualCharacterHeight},{this.IndividualCharacterWidth}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new ChangeAlphanumericDefaultFontCommand();
            var zplDataParts = zplCommand.Substring(CommandPrefix.Length).Split(',');

            if (zplDataParts.Length > 0)
            {
                command.SpecifiedDefaultFont = zplDataParts[0][0];
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var individualCharacterHeight))
                {
                    command.IndividualCharacterHeight = individualCharacterHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var individualCharacterWidth))
                {
                    command.IndividualCharacterWidth = individualCharacterWidth;
                }
            }

            return command;
        }

    }
}
