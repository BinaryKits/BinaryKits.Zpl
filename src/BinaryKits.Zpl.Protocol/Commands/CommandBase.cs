using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Protocol.Commands
{
    /// <summary>
    /// Command Base
    /// </summary>
    public abstract class CommandBase
    {
        private static readonly IEnumerable<(Func<string, bool> canParse, Func<string, CommandBase> parse)> Parsers =
            typeof(CommandBase).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(CommandBase)))
            .Select(type => (
                (Func<string, bool>)Delegate.CreateDelegate(typeof(Func<string, bool>), type.GetMethod("CanParseCommand", BindingFlags.Public | BindingFlags.Static)),
                (Func<string, CommandBase>)Delegate.CreateDelegate(typeof(Func<string, CommandBase>), type.GetMethod("ParseCommand", BindingFlags.Public | BindingFlags.Static))));

        /// <summary>
        /// A regex that matches an optional storage location with filename
        /// </summary>
        protected static readonly Regex StorageFileNameRegex = new Regex(@"^(\w:)?(.+\..+)$", RegexOptions.Compiled);

        /// <summary>
        /// The Command Prefix
        /// </summary>
        protected static readonly string CommandPrefix;

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
        public static bool CanParseCommand(string zplCommand)
        {
            return Parsers.Any(parser => parser.canParse(zplCommand));
        }

        /// <summary>
        /// Parse the Zpl Command
        /// </summary>
        /// <param name="zplCommand"></param>
        public static CommandBase ParseCommand(string zplCommand)
        {
            var validParser = Parsers.Where(parser => parser.canParse(zplCommand)).SingleOrDefault();

            if (!validParser.Equals(default))
            {
                return validParser.parse(zplCommand);
            }

            return null;
        }

        /// <summary>
        /// Validate integer paramter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="number"></param>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        protected static bool ValidateIntParameter(string parameterName, int? number, int minimumValue, int maximumValue)
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
        /// Validate device to store image
        /// </summary>
        /// <param name="storageDevice"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected static bool ValidateStorageDevice(string storageDevice)
        {
            if (storageDevice.Length != 2)
            {
                throw new ArgumentException($"Invalid format requires 2 characters", storageDevice);
            }

            var allowedDevices = new char[] { 'R', 'E', 'B', 'A' };

            if (!allowedDevices.Contains(storageDevice[0]))
            {
                throw new ArgumentException($"Invalid device letter", storageDevice);
            }

            if (storageDevice[1] != ':')
            {
                throw new ArgumentException($"The second character must be a colon", storageDevice);
            }

            return true;
        }

        /// <summary>
        /// Get Zpl char for line color
        /// </summary>
        /// <param name="lineColor"></param>
        /// <returns></returns>
        protected static string RenderLineColor(LineColor lineColor)
        {
            switch (lineColor)
            {
                case LineColor.Black:
                    return "B";
                case LineColor.White:
                    return "W";
            }

            throw new ArgumentException("Unknown Line Color");
        }

        /// <summary>
        /// Render Zpl char for boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RenderBoolean(bool value)
        {
            return value ? "Y" : "N";
        }

        /// <summary>
        /// Get Zpl char for orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        protected static string RenderOrientation(Orientation orientation)
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

            throw new ArgumentException("Unknown Orientation");
        }

        /// <summary>
        /// Get Zpl char for error correction level
        /// </summary>
        /// <param name="errorCorrectionLevel"></param>
        /// <returns></returns>
        public static string RenderErrorCorrectionLevel(ErrorCorrectionLevel errorCorrectionLevel)
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

            throw new ArgumentException("Unknown Error Correction Level");
        }

        /// <summary>
        /// Get Zpl char for text justification
        /// </summary>
        /// <param name="textJustification"></param>
        /// <returns></returns>
        public static string RenderTextJustification(TextJustification textJustification)
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

            throw new ArgumentException("Unknown Text Justification");
        }

        /// <summary>
        /// Get boolean from ZPL Char
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertBoolean(string value)
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
        protected static Orientation ConvertOrientation(string orientation)
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
        protected static ErrorCorrectionLevel ConvertErrorCorrectionLevel(string errorCorrectionLevel)
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

    }
}
