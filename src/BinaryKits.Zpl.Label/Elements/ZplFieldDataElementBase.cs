using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplFieldDataElementBase : ZplPositionedElementBase, IFormatElement
    {

        public string Content { get; protected set; }

        public FieldOrientation FieldOrientation { get; protected set; }

        public bool UseHexadecimalIndicator { get; protected set; }

        public bool IsDigitsOnly => this.Content.All(char.IsDigit);

        public ZplFieldDataElementBase(
            string content,
            int positionX,
            int positionY,
            FieldOrientation fieldOrientation,
            bool useHexadecimalIndicator,
            bool bottomToTop)
            : base(positionX, positionY, bottomToTop)
        {
            Content = content;
            FieldOrientation = fieldOrientation;
            UseHexadecimalIndicator = useHexadecimalIndicator;
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
            if (UseHexadecimalIndicator)
            {
                sb.Append("^FH");
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
