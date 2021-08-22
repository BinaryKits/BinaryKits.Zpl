using System;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Command Base
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// The Command Prefix
        /// </summary>
        protected string CommandPrefix { get; private set; }

        /// <summary>
        /// Command Base
        /// </summary>
        /// <param name="commandPrefix"></param>
        public CommandBase(string commandPrefix)
        {
            this.CommandPrefix = commandPrefix;
        }

        /// <summary>
        /// Split a zpl command in data parts
        /// </summary>
        /// <param name="zplCommand"></param>
        /// <param name="dataStartIndex"></param>
        /// <returns></returns>
        protected string[] SplitCommand(string zplCommand, int dataStartIndex = 0)
        {
            var zplCommandData = zplCommand.Substring(this.CommandPrefix.Length + dataStartIndex);
            return zplCommandData.Split(',');
        }

        /// <summary>
        /// Get the Zpl Command
        /// </summary>
        /// <returns></returns>
        public abstract string ToZpl();

        /// <summary>
        /// Check the zpl command is parsable
        /// </summary>
        /// <param name="zplCommand"></param>
        /// <returns></returns>
        public bool IsCommandParsable(string zplCommand)
        {
            return zplCommand.StartsWith(this.CommandPrefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Parse the Zpl Command
        /// </summary>
        /// <param name="zplCommand"></param>
        public abstract void ParseCommand(string zplCommand);

        /// <summary>
        /// Validate integer paramter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="number"></param>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        protected bool ValidateIntParameter(string parameterName, int? number, int minimumValue, int maximumValue)
        {
            if (number.HasValue)
            {
                if (number.Value < minimumValue || number.Value > maximumValue)
                {
                    throw new ArgumentException($"Must be between {minimumValue} and {maximumValue}", parameterName);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get Zpl Line color
        /// </summary>
        /// <param name="lineColor"></param>
        /// <returns></returns>
        protected string RenderLineColor(LineColor lineColor)
        {
            switch (lineColor)
            {
                case LineColor.Black:
                    return "B";
                case LineColor.White:
                    return "W";
            }

            throw new NotImplementedException("Unknown Line Color");
        }
    }
}
