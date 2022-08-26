using System;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Protocol.Commands
{
    public class FieldNumberCommand : CommandBase
    {
        ///<inheritdoc/>
        protected static new readonly string CommandPrefix = "^FN";

        /// <summary>
        /// Number to be assigned to the field. Accepted Values: 0 to 9999
        /// </summary>
        public int AssignedNumber { get; private set; } = 0;

        /// <summary>
        /// The optional prompt display parameter can be used with the KDU Plus to cause prompts to be displayed
        /// on the KDU unit. Also, when the Print on Label link is selected on the Directory page of
        /// ZebraLink enabled printers the field prompt displays.
        /// </summary>
        public string PromptDisplay { get; private set; } = "optional parameter";

        /// <summary>
        /// Field Separator
        /// </summary>
        public FieldNumberCommand()
        { }

        /// <summary>
        /// Field Separator
        /// </summary>
        /// <param name="assignedNumber">Number to be assigned to the field</param>
        /// <param name="promptDisplay">Prompt display (ignored)</param>
        public FieldNumberCommand(int assignedNumber, string promptDisplay = "optional parameter")
        {
            this.AssignedNumber = assignedNumber;
            this.PromptDisplay = promptDisplay;
        }

        ///<inheritdoc/>
        public override string ToZpl()
        {
            // NB. PromptDisplay is ignored
            return $"{CommandPrefix}{this.AssignedNumber}";
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return zplCommand.StartsWith(CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var command = new FieldNumberCommand();
            var zplCommandData = zplCommand.Substring(CommandPrefix.Length);
            var fieldNumberDataMatch = Regex.Match(zplCommandData, @"^(\d+)?(?:""([a-zA-Z1-9 ]*)"")?$");

            if (fieldNumberDataMatch.Success)
            {
                if (fieldNumberDataMatch.Groups[1].Success)
                {
                    command.AssignedNumber = int.Parse(fieldNumberDataMatch.Groups[1].Value);
                }

                if (fieldNumberDataMatch.Groups[2].Success)
                {
                    command.PromptDisplay = fieldNumberDataMatch.Groups[2].Value;
                }
            }

            return command;
        }

    }
}
