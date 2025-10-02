using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplAztecBarcode : ZplFieldDataElementBase
    {
        public int MagnificationFactor { get; protected set; }
        public bool ExtendedChannel { get; protected set; }
        public int ErrorControl { get; protected set; }
        public bool MenuSymbol { get; protected set; }
        public int SymbolCount { get; protected set; }
        public string IdField { get; protected set; }

        /// <summary>
        /// Aztec Bar Code
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="magnificationFactor"></param>
        /// <param name="extendedChannel">extended channel interpretation code indicator</param>
        /// <param name="errorControl">error control and symbol size/type indicator</param>
        /// <param name="menuSymbol">menu symbol indicator</param>
        /// <param name="symbolCount">number of symbols for structured append</param>
        /// <param name="idField">optional ID field for structured append</param>
        /// <param name="useHexadecimalIndicator"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="bottomToTop"></param>
        public ZplAztecBarcode(
            string content,
            int positionX,
            int positionY,
            int magnificationFactor = 2,
            bool extendedChannel = false,
            int errorControl = 0,
            bool menuSymbol = false,
            int symbolCount = 1,
            string idField = null,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool useHexadecimalIndicator = false,
            bool bottomToTop = false)
            : base(content, positionX, positionY, fieldOrientation, useHexadecimalIndicator, bottomToTop)
        {
            this.MagnificationFactor = magnificationFactor;
            this.ExtendedChannel = extendedChannel;
            this.ErrorControl = errorControl;
            this.SymbolCount = symbolCount;
            this.IdField = idField;
        }
 
        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BO{RenderFieldOrientation(this.FieldOrientation)},{this.MagnificationFactor},{RenderBoolean(this.ExtendedChannel)}," +
                $"{this.ErrorControl},{RenderBoolean(this.MenuSymbol)},{this.SymbolCount},{this.IdField}");
            result.Add(RenderFieldDataSection());

            return result;
        }
    }
}
