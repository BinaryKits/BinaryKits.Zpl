using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplReferenceGrid : ZplElementBase
    {
        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var gridSize = 100;
            var font = new ZplFont(fontWidth: 0, fontHeight: 20, fontName: "0");

            var result = new List<string>();

            for (var x = 0; x < 30; x++)
            {
                for (var y = 0; y < 30; y++)
                {
                    var positionX = x * gridSize;
                    var positionY = y * gridSize;

                    result.AddRange(new ZplGraphicBox(positionX, positionY, gridSize, gridSize).Render());
                    result.AddRange(new ZplTextField($"X:{positionX}", positionX + 10, positionY + 30, font).Render());
                    result.AddRange(new ZplTextField($"Y:{positionY}", positionX + 10, positionY + 50, font).Render());
                }
            }

            return result;
        }
    }
}
