using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Data Matrix Bar Code, ^BXo,h,s,c,r,f,g,a
    /// </summary>
    public class ZplDataMatrix : ZplPositionedElementBase, IFormatElement
    {
        /// <summary>
        /// Data Matrix Bar Code
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="bottomToTop"></param>
        public ZplDataMatrix(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            bool bottomToTop = false
           )
            : base(positionX, positionY, bottomToTop)
        {
            Content = content;
            FieldOrientation = fieldOrientation;
            Height = height;
        }

        public int Height { get; protected set; }

        public FieldOrientation FieldOrientation { get; protected set; }

        public string Content { get; protected set; }

        protected string RenderFieldOrientation()
        {
            return RenderFieldOrientation(FieldOrientation);
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
            result.Add($"^FD{Content}^FS");

            return result;
        }

        /// <inheritdoc />
        public void SetTemplateContent(string content)
        {
            Content = content;
        }
    }
}
