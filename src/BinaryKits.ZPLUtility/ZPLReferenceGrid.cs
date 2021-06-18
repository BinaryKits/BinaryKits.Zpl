using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
    public class ZPLReferenceGrid : ZPLElementBase
    {
        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var result = new List<string>();

            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZPLGraphicBox(0, i, 3000, 1).Render());
            }

            for (int i = 0; i <= 1500; i += 100)
            {
                result.AddRange(new ZPLGraphicBox(i, 0, 1, 3000).Render());
            }

            return result;
        }
    }
}
