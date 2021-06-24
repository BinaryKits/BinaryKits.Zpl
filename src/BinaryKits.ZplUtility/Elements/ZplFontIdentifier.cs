using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplFontIdentifier : ZplElementBase
    {
        public string FontReplaceLetter { get; set; }
        public string Device { get; set; }
        public string FontFileName { get; set; }

        public ZplFontIdentifier(string fontReplaceLetter, string device, string fontFileName)
        {
            FontReplaceLetter = fontReplaceLetter;
            Device = device;
            FontFileName = fontFileName;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
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
