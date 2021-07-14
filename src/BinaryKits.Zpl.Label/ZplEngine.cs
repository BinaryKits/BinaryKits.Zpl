using BinaryKits.Zpl.Label.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryKits.Zpl.Label
{
    public class ZplEngine : List<ZplElementBase>
    {
        /// <summary>
        /// Start an empty engine
        /// </summary>
        public ZplEngine() { }

        /// <summary>
        /// Start an engine with given elements
        /// </summary>
        /// <param name="elements">Zpl elements to be added</param>
        public ZplEngine(IEnumerable<ZplElementBase> elements) : base(elements) { }

        /// <summary>
        /// Output the Zpl string using given context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();

            if (context.AddStartEndFormat)
            {
                result.Add("^XA");
            }

            if (context.AddDefaultLabelHome)
            {
                result.Add("^LH0,0");
            }

            result.Add(context.ChangeInternationalFontEncoding);

            foreach (var element in this.Where(x => x.IsEnabled))
            {
                //Empty line
                if (context.AddEmptyLineBeforeElementStart)
                {
                    result.Add("");
                }

                //Comments
                if (context.DisplayComments)
                {
                    if (element.Comments.Any())
                    {
                        result.Add("^FX");
                        element.Comments.ForEach(x => result.Add("//" + x.Replace("^", "[caret]").Replace("~", "[tilde]")));
                    }
                }

                //Actual element
                if (context.CompressedRendering)
                {
                    result.Add(string.Join("", element.Render(context)));
                }
                else
                {
                    result.AddRange(element.Render(context));
                }
            }

            if (context.AddStartEndFormat)
            {
                result.Add("^XZ");
            }

            return result;
        }

        public string ToZplString(ZplRenderOptions context)
        {
            return string.Join("\n", Render(context));
        }

        /// <summary>
        /// Add raw Zpl fragment
        /// </summary>
        /// <param name="rawZplCode"></param>
        public void AddRawZplCode(string rawZplCode)
        {
            Add(new ZplRaw(rawZplCode));
        }

        /// <summary>
        /// Convert a char to be Hex value, 2 letters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string MapToHexadecimalValue(char input)
        {
            return Convert.ToByte(input).ToString("X2");
        }
    }
}
