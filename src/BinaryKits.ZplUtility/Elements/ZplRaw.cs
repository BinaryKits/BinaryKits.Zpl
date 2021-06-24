using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplRaw : ZplElementBase
    {
        public string RawContent { get; set; }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            return new[] { RawContent };
        }

        public ZplRaw(string rawZplCode)
        {
            RawContent = rawZplCode;
        }
    }
}
