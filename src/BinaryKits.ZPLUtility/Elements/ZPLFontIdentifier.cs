using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.ZPLUtility.Elements
{
    public class ZPLFontIdentifier : ZPLElementBase
    {
        public string FontReplaceLetter { get; set; }
        public string Device { get; set; }
        public string FontFileName { get; set; }

        public ZPLFontIdentifier(string fontReplaceLetter, string device, string fontFileName)
        {
            FontReplaceLetter = fontReplaceLetter;
            Device = device;
            FontFileName = fontFileName;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^CWa,d:o.x
            var result = new List<string>
            {
                $"^CW{FontReplaceLetter},{Device}:{FontFileName}"
            };

            return result;
        }
    }
}
