using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplReferenceGrid : ZplElementBase
    {
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();

            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZplGraphicBox(0, i, 3000, 1).Render());
            }

            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZplGraphicBox(i, 0, 1, 3000).Render());
            }

            return result;
        }
    }
}
