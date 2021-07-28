using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Field Typeset<br/>
    /// The ^FT command sets the field position, relative to the home position of the label designated by the ^LH
    /// command.The typesetting origin of the field is fixed with respect to the contents of the field and does not
    /// change with rotation.
    /// </summary>
    public class ZplFieldTypeset : ZplElementBase
    {
        public int PositionX { get; protected set; }
        public int PositionY { get; protected set; }

        /// <summary>
        /// Field Typeset
        /// </summary>
        /// <param name="positionX">X Position (0-32000)</param>
        /// <param name="positionY">Y Position (0-32000)</param>
        public ZplFieldTypeset(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO50,50
            return new string[] { $"^FT{context.Scale(PositionX)},{context.Scale(PositionY)}" };
        }

        /// <summary>
        /// Return a new instance with offset applied
        /// </summary>
        /// <param name="offsetX">Offset on x</param>
        /// <param name="offsetY">Offset on y</param>
        /// <returns></returns>
        public ZplFieldTypeset Offset(int offsetX, int offsetY)
        {
            return new ZplFieldTypeset(PositionX + offsetX, PositionY + offsetY);
        }
    }
}
