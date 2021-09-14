namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// FieldBlock<br/>
    /// The ^FB command allows you to print text into a defined block type format. This command formats an ^FD
    /// or ^SN string into a block of text using the origin, font, and rotation specified for the text string. The ^FB
    /// command also contains an automatic word-wrap function.
    /// </summary>
    public class FieldBlockCommand : CommandBase
    {
        /// <summary>
        /// Width of text block line
        /// </summary>
        public int WidthOfTextBlockLine { get; private set; }

        /// <summary>
        /// Maximum number of lines in text block
        /// </summary>
        public int MaximumNumberOfLinesInTextBlock { get; private set; } = 1;

        /// <summary>
        /// Add or delete space between lines
        /// </summary>
        public int AddOrDeleteSpaceBetweenLines { get; private set; }

        /// <summary>
        /// Text justification
        /// </summary>
        public TextJustification TextJustification { get; private set; } = TextJustification.Left;

        /// <summary>
        /// Hanging indent of the second and remaining lines
        /// </summary>
        public int HangingIndentOfTheSecondAndRemainingLines { get; private set; }

        /// <summary>
        /// Field Block
        /// </summary>
        public FieldBlockCommand() : base("^FB")
        { }

        /// <summary>
        /// Field Block
        /// </summary>
        /// <param name="widthOfTextBlockLine">Width of text block line</param>
        /// <param name="maximumNumberOfLinesInTextBlock">Maximum number of lines in text block</param>
        /// <param name="addOrDeleteSpaceBetweenLines">Add or delete space between lines</param>
        /// <param name="textJustification">Text justification</param>
        /// <param name="hangingIndentOfTheSecondAndRemainingLines">Hanging indent of the second and remaining lines</param>
        public FieldBlockCommand(
            int widthOfTextBlockLine = 0,
            int maximumNumberOfLinesInTextBlock = 1,
            int addOrDeleteSpaceBetweenLines = 0,
            TextJustification textJustification = TextJustification.Left,
            int hangingIndentOfTheSecondAndRemainingLines = 0)
            : this()
        {
            this.WidthOfTextBlockLine = widthOfTextBlockLine;

            if (this.ValidateIntParameter(nameof(maximumNumberOfLinesInTextBlock), maximumNumberOfLinesInTextBlock, 1, 9999))
            {
                this.MaximumNumberOfLinesInTextBlock = maximumNumberOfLinesInTextBlock;
            }

            if (this.ValidateIntParameter(nameof(addOrDeleteSpaceBetweenLines), addOrDeleteSpaceBetweenLines, -9999, 9999))
            {
                this.AddOrDeleteSpaceBetweenLines = addOrDeleteSpaceBetweenLines;
            }

            this.TextJustification = textJustification;

            if (this.ValidateIntParameter(nameof(hangingIndentOfTheSecondAndRemainingLines), hangingIndentOfTheSecondAndRemainingLines, 0, 9999))
            {
                this.HangingIndentOfTheSecondAndRemainingLines = hangingIndentOfTheSecondAndRemainingLines;
            }
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            return $"{this.CommandPrefix}{this.WidthOfTextBlockLine},{this.MaximumNumberOfLinesInTextBlock},{this.AddOrDeleteSpaceBetweenLines},{this.RenderTextJustification(this.TextJustification)},{this.HangingIndentOfTheSecondAndRemainingLines}";
        }

        ///<inheritdoc/>
        public override void ParseCommand(string zplCommand)
        {
            var zplDataParts = this.SplitCommand(zplCommand);

            if (zplDataParts.Length > 0)
            {
                if (int.TryParse(zplDataParts[0], out var widthOfTextBlockLine))
                {
                    this.WidthOfTextBlockLine = widthOfTextBlockLine;
                }
            }
            if (zplDataParts.Length > 1)
            {
                if (int.TryParse(zplDataParts[1], out var maximumNumberOfLinesInTextBlock))
                {
                    this.MaximumNumberOfLinesInTextBlock = maximumNumberOfLinesInTextBlock;
                }
            }
            if (zplDataParts.Length > 2)
            {
                if (int.TryParse(zplDataParts[2], out var addOrDeleteSpaceBetweenLines))
                {
                    this.AddOrDeleteSpaceBetweenLines = addOrDeleteSpaceBetweenLines;
                }
            }
            if (zplDataParts.Length > 3)
            {
                switch (zplDataParts[3])
                {
                    case "C":
                        this.TextJustification = TextJustification.Center;
                        break;
                    case "R":
                        this.TextJustification = TextJustification.Right;
                        break;
                    case "J":
                        this.TextJustification = TextJustification.Justified;
                        break;
                }
            }
            if (zplDataParts.Length > 4)
            {
                if (int.TryParse(zplDataParts[4], out var hangingIndentOfTheSecondAndRemainingLines))
                {
                    this.HangingIndentOfTheSecondAndRemainingLines = hangingIndentOfTheSecondAndRemainingLines;
                }
            }
        }
    }
}
