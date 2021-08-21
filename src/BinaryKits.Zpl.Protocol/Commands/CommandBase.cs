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
    }
}
