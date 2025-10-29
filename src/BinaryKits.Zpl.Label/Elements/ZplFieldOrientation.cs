using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// ^FW - Field Orientation
    /// </summary>
    public class ZplFieldOrientation : ZplElementBase
    {
        public FieldOrientation FieldOrientation { get; private set; }
        public FieldJustification FieldJustification { get; private set; }

        /// <summary>
        /// Field Orientation
        /// </summary>
        /// <param name="fieldOrientation"></param>
        /// <param name="fieldJustification"></param>
        public ZplFieldOrientation(FieldOrientation fieldOrientation, FieldJustification fieldJustification = FieldJustification.None)
        {
            this.FieldOrientation = fieldOrientation;
            this.FieldJustification = fieldJustification;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            return new[] { $"^FW{RenderFieldOrientation(this.FieldOrientation)},{RenderFieldJustification(this.FieldJustification)}".TrimEnd(',') };
        }

    }
}
