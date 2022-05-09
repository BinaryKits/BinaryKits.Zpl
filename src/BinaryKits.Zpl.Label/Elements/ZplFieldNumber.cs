using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Field number element as declared in formats (templates).
    /// </summary>
    /// <see cref="ZplRecallFieldNumber"/>
    public class ZplFieldNumber : ZplElementBase
    {
        public int Number { get; private set; }
        public ZplElementBase FormatElement { get; private set; }

        /// <summary>
        /// Field number with format element (without data).
        /// </summary>
        /// <param name="number">Field number from 0 to 9999</param>
        /// <param name="formatElement">Element providing location, font</param>
        /// <exception cref="ArgumentException">If element is not an <see cref="IFormatElement"/>
        /// (allowing it's data to be set when merging formats)</exception>
        public ZplFieldNumber(int number, ZplElementBase formatElement)
        {
            if (!(formatElement is IFormatElement))
            {
                throw new ArgumentException("", nameof(formatElement));
            }

            Number = number;
            FormatElement = formatElement;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(FormatElement.Render(context));
            result.Add($"^FN{Number}^FS");
            return result;
        }
    }
}
