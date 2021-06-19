using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    /// <summary>
    /// Field Origin<br/>
    /// The ^FO command sets a field origin, relative to the label home (^LH) position.
    /// ^FO sets the upper-left corner of the field area by defining points along the x-axis and y-axis independent of the rotation.
    /// </summary>
    /// <remarks>
    /// Format:^FOx,y
    /// </remarks>
    public class ZPLOrigin : ZPLElementBase
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }

        /// <summary>
        /// ZPLOrigin
        /// </summary>
        /// <param name="positionX">X Position (0-32000)</param>
        /// <param name="positionY">Y Position (0-32000)</param>
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
