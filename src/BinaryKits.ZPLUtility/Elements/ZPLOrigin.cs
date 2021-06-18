using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    //^FO – Field Origin
    public class ZPLOrigin : ZPLElementBase
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }

        public ZPLOrigin(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //^ FO50,50
            return new string[] { $"^FO{context.Scale(PositionX)},{context.Scale(PositionY)}" };
        }

        /// <summary>
        /// Return a new instance with offset applied
        /// </summary>
        /// <param name="OffsetX"></param>
        /// <param name="OffsetY"></param>
        /// <returns></returns>
        public ZPLOrigin Offset(int OffsetX, int OffsetY)
        {
            return new ZPLOrigin(PositionX + OffsetX, PositionY + OffsetY);
        }
    }
}
