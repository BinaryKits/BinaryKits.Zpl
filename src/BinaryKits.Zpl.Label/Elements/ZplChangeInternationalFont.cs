using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// ^CI - Change International Font/Encoding
    /// </summary>
    public class ZplChangeInternationalFont : ZplElementBase
    {
        public InternationalFont InternationalFont { get; private set; }

        public ZplChangeInternationalFont(InternationalFont internationalFont)
        {
            this.InternationalFont = internationalFont;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            return new[] { $"^CI{(int)this.InternationalFont}" };
        }
    }

}
