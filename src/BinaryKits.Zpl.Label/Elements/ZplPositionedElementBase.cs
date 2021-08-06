using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplPositionedElementBase : ZplElementBase
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public ZplFieldOrigin FieldOrigin { get; private set; }
        public ZplFieldTypeset FieldTypeset { get; private set; }

        /// <summary>
        /// ZplPositionedElementBase
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="bottomToTop">Use FieldTypeset</param>
        public ZplPositionedElementBase(int positionX, int positionY, bool bottomToTop = false) : base()
        {
            if (bottomToTop)
            {
                FieldTypeset = new ZplFieldTypeset(positionX, positionY);
                PositionX = positionX;
                PositionY = positionY;
                return;
            }

            FieldOrigin = new ZplFieldOrigin(positionX, positionY);
            PositionX = positionX;
            PositionY = positionY;
        }

        public IEnumerable<string> RenderPosition(ZplRenderOptions context)
        {
            if (FieldOrigin != null)
            {
                return FieldOrigin.Render(context);
            }

            return FieldTypeset.Render(context);
        }
    }
}
