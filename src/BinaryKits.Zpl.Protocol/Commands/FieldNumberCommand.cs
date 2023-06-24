using SixLabors.ImageSharp;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Field Number<br/>
    /// The ^FN command numbers the data fields. This command is used in both
    /// ^DF(Store Format) and ^XF(Recall Format) commands.
    /// </summary>
    [CommandPrefix("^FN")]
    public class FieldNumberCommand : CommandBase
    {
        /// <summary>
        /// Number to be assigned to the field. Accepted Values: 0 to 9999
        /// </summary>
        [ZplDataMember(Order = 0, PreceedingDelimiter = "")]
        [DefaultValue(0)]
        public int AssignedNumber { get; private set; } = 0;

        /// <summary>
        /// The optional prompt display parameter can be used with the KDU Plus to cause prompts to be displayed
        /// on the KDU unit. Also, when the Print on Label link is selected on the Directory page of
        /// ZebraLink enabled printers the field prompt displays.
        /// </summary>
        [ZplDataMember(Order = 1, PreceedingDelimiter = "", FormatString = "\"{0}\"")]
        [DefaultValue("optional parameter")]
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
            var attribute = this.GetType().GetCustomAttribute(typeof(CommandPrefixAttribute));
            if (attribute is CommandPrefixAttribute prefixAttribute)
            {
                // NB. PromptDisplay is ignored
                return $"{prefixAttribute.Prefix}{this.AssignedNumber}";
            }

            return string.Empty;
        }

        ///<inheritdoc/>
        public static new bool CanParseCommand(string zplCommand)
        {
            return CommandBase.CanParseCommand<FieldNumberCommand>(zplCommand);
        }

        ///<inheritdoc/>
        public static new CommandBase ParseCommand(string zplCommand)
        {
            var attribute = typeof(FieldNumberCommand).GetCustomAttribute(typeof(CommandPrefixAttribute));
            if (attribute is CommandPrefixAttribute prefixAttribute)
            {
                var command = new FieldNumberCommand();
                var zplCommandData = zplCommand.Substring(prefixAttribute.Prefix.Length);
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

            return null;
        }

    }
}
