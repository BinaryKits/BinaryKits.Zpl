using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Data Matrix Bar Code, ^BXo,h,s,c,r,f,g,a
    /// </summary>
    public class ZplDataMatrix : ZplFieldDataElementBase
    {
        public int Height { get; protected set; }

        /// <summary>
        /// Data Matrix Bar Code
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="useHexadecimalIndicator"></param>
        /// <param name="bottomToTop"></param>
        public ZplDataMatrix(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool useHexadecimalIndicator = false,
            bool bottomToTop = false
           )
            : base(content, positionX, positionY, fieldOrientation, useHexadecimalIndicator, bottomToTop)
        {
            Height = height;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100
            //^BXN,10,200
            //^FDZEBRA TECHNOLOGIES CORPORATION ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BX{RenderFieldOrientation()},{context.Scale(Height)}");
            result.Add(RenderFieldDataSection());

            return result;
        }

    }
}
