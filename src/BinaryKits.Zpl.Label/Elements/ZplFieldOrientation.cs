using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// ^FW - Field Orientation
    /// </summary>
    public class ZplFieldOrientation : ZplElementBase
    {
        public FieldOrientation FieldOrientation { get; private set; }

        public ZplFieldOrientation(FieldOrientation fieldOrientation)
        {
            this.FieldOrientation = fieldOrientation;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            return new[] { $"^FW{RenderFieldOrientation(this.FieldOrientation)}" };
        }

    }
}
