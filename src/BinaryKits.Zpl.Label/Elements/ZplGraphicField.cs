using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
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
            char compressionType = 'A')
            : base(positionX, positionY)
        {
            this.CompressionType = compressionType;
            this.BinaryByteCount = binaryByteCount;
            this.GraphicFieldCount = graphicFieldCount;
            this.BytesPerRow = bytesPerRow;
            this.Data = data;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            throw new NotImplementedException();
        }
    }
}
