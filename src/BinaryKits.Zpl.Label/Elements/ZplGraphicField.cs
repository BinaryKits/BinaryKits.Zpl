using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Graphic Field - ^GF
    /// </summary>
    public class ZplGraphicField : ZplPositionedElementBase
    {
        public char CompressionType { get; private set; }
        public int BinaryByteCount { get; private set; }
        public int GraphicFieldCount { get; private set; }
        public int BytesPerRow { get; private set; }
        public string Data { get; private set; }

        public ZplGraphicField(
            int positionX,
            int positionY,
            int binaryByteCount,
            int graphicFieldCount,
            int bytesPerRow,
            string data,
            bool bottomToTop = false,
            char compressionType = 'A')
            : base(positionX, positionY, bottomToTop)
        {
            CompressionType = compressionType;
            BinaryByteCount = binaryByteCount;
            GraphicFieldCount = graphicFieldCount;
            BytesPerRow = bytesPerRow;
            Data = data;
        }

        /// <summary>
        /// Render (^GFa,b,c,d,data)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^GF{CompressionType},{BinaryByteCount},{GraphicFieldCount},{BytesPerRow},{Data}");

            return result;
        }
    }
}
