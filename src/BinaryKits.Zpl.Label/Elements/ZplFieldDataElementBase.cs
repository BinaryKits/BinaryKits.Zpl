using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplFieldDataElementBase : ZplPositionedElementBase, IFormatElement
    {

        public string Content { get; protected set; }

        public FieldOrientation FieldOrientation { get; protected set; }

        public char? HexadecimalIndicator { get; protected set; }

        public ZplFieldDataElementBase(
            string content,
            int positionX,
            int positionY,
            FieldOrientation fieldOrientation,
            char? hexadecimalIndicator,
            bool bottomToTop,
            bool useDefaultPosition = false)
            : base(positionX, positionY, bottomToTop, useDefaultPosition: useDefaultPosition)
        {
            Content = content;
            FieldOrientation = fieldOrientation;
            HexadecimalIndicator = hexadecimalIndicator;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add(RenderFieldDataSection());

            return result;
        }

        protected string RenderFieldOrientation()
        {
            return RenderFieldOrientation(FieldOrientation);
        }

        protected string RenderFieldDataSection()
        {
            var sb = new StringBuilder();
            if (HexadecimalIndicator is char hexIndicator)
            {
                sb.Append("^FH");
                if (hexIndicator != '_')
                {
                    sb.Append(hexIndicator);
                }
            }

            sb.Append("^FD");
            sb.Append(this.Content);
            sb.Append("^FS");

            return sb.ToString();
        }

        /// <inheritdoc />
        public void SetTemplateContent(string content)
        {
            Content = content;
        }

    }
}
