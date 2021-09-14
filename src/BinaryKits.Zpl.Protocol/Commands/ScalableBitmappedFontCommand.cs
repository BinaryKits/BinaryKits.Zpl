namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Scalable/Bitmapped Font<br/>
    /// The ^A command specifies the font to use in a text field. ^A designates the font for the current ^FD
    /// statement or field.The font specified by ^A is used only once for that ^FD entry.If a value for ^A is not
    /// specified again, the default ^CF font is used for the next ^FD entry.
    /// </summary>
    public class ScalableBitmappedFontCommand : CommandBase
    {
        /// <summary>
        /// Font name
        /// </summary>
        public char FontName { get; private set; }

        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation { get; private set; }

        /// <summary>
        /// Character Height
        /// </summary>
        public int? CharacterHeight { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int? Width { get; private set; }

        /// <summary>
        /// Scalable/Bitmapped Font
        /// </summary>
        public ScalableBitmappedFontCommand() : base("^A")
        { }

        /// <summary>
        /// Scalable/Bitmapped Font
        /// </summary>
        /// <param name="fontName">Font name, A through Z, and 0 to 9</param>
        /// <param name="orientation">Orientation</param>
        /// <param name="characterHeight">Character Height (10 to 32000)</param>
        /// <param name="width">width (10 to 32000)</param>
        public ScalableBitmappedFontCommand(
            char fontName,
            Orientation orientation = Orientation.Normal,
            int? characterHeight = 10,
            int? width = 10)
            : this()
        {
            this.FontName = fontName;
            this.Orientation = orientation;

            if (this.ValidateIntParameter(nameof(characterHeight), characterHeight, 10, 32000))
            {
                this.CharacterHeight = characterHeight.Value;
            }

            if (this.ValidateIntParameter(nameof(width), width, 10, 32000))
            {
                this.Width = width.Value;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.FontName}{this.RenderOrientation(this.Orientation)},{this.CharacterHeight},{this.Width}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            this.FontName = zplCommand[this.CommandPrefix.Length];

            var zplDataParts = this.SplitCommand(zplCommand, 1);

            if (zplDataParts.Length > 0)
            {
                this.Orientation = this.ConvertOrientation(zplDataParts[0]);
            }

            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var characterHeight))
                {
                    this.CharacterHeight = characterHeight;
                }
            }

            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var width))
                {
                    this.Width = width;
                }
            }
        }
    }
}
