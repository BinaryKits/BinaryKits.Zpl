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
        /// Get Zpl char for line color
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

        /// <summary>
        /// Render Zpl char for boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string RenderBoolean(bool value)
        {
            return value ? "Y" : "N";
        }

        /// <summary>
        /// Get Zpl char for orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        protected string RenderOrientation(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Normal:
                    return "N";
                case Orientation.Rotated90:
                    return "R";
                case Orientation.Rotated180:
                    return "I";
                case Orientation.Rotated270:
                    return "B";
            }

            throw new NotImplementedException("Unknown Orientation");
        }

        /// <summary>
        /// Get Zpl char for error correction level
        /// </summary>
        /// <param name="errorCorrectionLevel"></param>
        /// <returns></returns>
        public string RenderErrorCorrectionLevel(ErrorCorrectionLevel errorCorrectionLevel)
        {
            switch (errorCorrectionLevel)
            {
                case ErrorCorrectionLevel.UltraHighReliability:
                    return "H";
                case ErrorCorrectionLevel.HighReliability:
                    return "Q";
                case ErrorCorrectionLevel.Standard:
                    return "M";
                case ErrorCorrectionLevel.HighDensity:
                    return "L";
            }

            throw new NotImplementedException("Unknown Error Correction Level");
        }

        /// <summary>
        /// Get Zpl char for text justification
        /// </summary>
        /// <param name="textJustification"></param>
        /// <returns></returns>
        public string RenderTextJustification(TextJustification textJustification)
        {
            switch (textJustification)
            {
                case TextJustification.Left:
                    return "L";
                case TextJustification.Center:
                    return "C";
                case TextJustification.Right:
                    return "R";
                case TextJustification.Justified:
                    return "J";
            }

            throw new NotImplementedException("Unknown Text Justification");
        }

        /// <summary>
        /// Get boolean from ZPL Char
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ConvertBoolean(string value)
        {
            if (value == "Y")
            {
                return true;
            }

            if (value == "N")
            {
                return false;
            }

            //Fallback
            return false;
        }

        /// <summary>
        /// Get orientation from Zpl char
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        protected Orientation ConvertOrientation(string orientation)
        {
            switch (orientation)
            {
                case "N":
                    return Orientation.Normal;
                case "R":
                    return Orientation.Rotated90;
                case "I":
                    return Orientation.Rotated180;
                case "B":
                    return Orientation.Rotated270;
            }

            //Fallback
            return Orientation.Normal;
        }

        /// <summary>
        /// Get error correction level from Zpl char
        /// </summary>
        /// <param name="errorCorrectionLevel"></param>
        /// <returns></returns>
        protected ErrorCorrectionLevel ConvertErrorCorrectionLevel(string errorCorrectionLevel)
        {
            switch (errorCorrectionLevel)
            {
                case "H":
                    return ErrorCorrectionLevel.UltraHighReliability;
                case "Q":
                    return ErrorCorrectionLevel.HighReliability;
                case "M":
                    return ErrorCorrectionLevel.Standard;
                case "L":
                    return ErrorCorrectionLevel.HighDensity;
            }

            //Fallback
            return ErrorCorrectionLevel.HighReliability;
        }

        /// <summary>
        /// Get the index of the specified character on the specified occurrence
        /// </summary>
        /// <param name="input"></param>
        /// <param name="occurranceToFind"></param>
        /// <param name="charToFind"></param>
        /// <returns></returns>
        protected int IndexOfNthCharacter(string input, int occurranceToFind, char charToFind)
        {
            var index = -1;
            for (var i = 0; i < occurranceToFind; i++)
            {
                index = input.IndexOf(charToFind, index + 1);

                if (index == -1)
                {
                    break;
                }
            }

            return index;
        }
    }
}
