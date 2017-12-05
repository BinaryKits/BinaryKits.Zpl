using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
    public class ZPLEngine : List<ZPLElementBase>
    {
        /// <summary>
        /// Start an empty engine
        /// </summary>
        public ZPLEngine() { }

        /// <summary>
        /// Start an engine with given elements
        /// </summary>
        /// <param name="elements">ZPL elements to be added</param>
        public ZPLEngine(IEnumerable<ZPLElementBase> elements) : base(elements) { }

        /// <summary>
        /// Output the ZPL string using given context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();

            if (context.AddStartEndFormat)
            {
                result.Add("^XA");
            }

            if (context.AddDefaultLabelHome)
            {
                result.Add("^LH0,0");
            }

            result.Add(context.ChangeInternationalFontEncoding);

            foreach (var e in this.Where(x => x.IsEnabled))
            {
                //Empty line
                if (context.AddEmptyLineBeforeElementStart)
                {
                    result.Add("");
                }

                //Comments
                if (context.DisplayComments)
                {
                    if (e.Comments.Any())
                    {
                        result.Add("^FX");
                        e.Comments.ForEach(x => result.Add("//" + x.Replace("^", "[caret]").Replace("~", "[tilde]")));
                    }
                }

                //Actual element
                if (context.CompressedRendering)
                {
                    result.Add(string.Join("", e.Render(context)));
                }
                else
                {
                    result.AddRange(e.Render(context));
                }
            }

            if (context.AddStartEndFormat)
            {
                result.Add("^XZ");
            }

            return result;
        }

        public string ToZPLString(ZPLRenderOptions context)
        {
            return string.Join("\n", Render(context));
        }

        /// <summary>
        /// Add raw ZPL fragment
        /// </summary>
        /// <param name="rawZPLCode"></param>
        public void AddRawZPLCode(string rawZPLCode)
        {
            Add(new ZPLRaw(rawZPLCode));
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
