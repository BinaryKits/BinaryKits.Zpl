using System.Collections.Generic;

namespace BinaryKits.ZPLUtility.Elements
{
    public class ZPLRaw : ZPLElementBase
    {
        public string RawContent { get; set; }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            return new[] { RawContent };
        }

        public ZPLRaw(string rawZPLCode)
        {
            RawContent = rawZPLCode;
        }
    }
}
