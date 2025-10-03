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
      
        public QualityLevel QualityLevel { get; protected set; }

        /// <summary>
        /// Data Matrix Bar Code
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="height"></param>
        /// <param name="fieldOrientation"></param>
        /// <param name="hexadecimalIndicator"></param>
        /// <param name="bottomToTop"></param>
        /// <param name="qualityLevel"></param>
        public ZplDataMatrix(
            string content,
            int positionX,
            int positionY,
            int height = 100,
            QualityLevel qualityLevel = QualityLevel.ECC0,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            char? hexadecimalIndicator = null,
            bool bottomToTop = false
           )
            : base(content, positionX, positionY, fieldOrientation, hexadecimalIndicator, bottomToTop)
        {
            Height = height;
            QualityLevel = qualityLevel;
        }

        protected string RenderQualityLevel()
        {
            return RenderQualityLevel(QualityLevel);
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100
            //^BXN,10,200
            //^FDZEBRA TECHNOLOGIES CORPORATION ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BX{RenderFieldOrientation()},{context.Scale(Height)},{RenderQualityLevel()}");
            result.Add($"^BX{RenderFieldOrientation()},{context.Scale(Height)}");
            result.Add(RenderFieldDataSection());

            return result;
        }

    }
}
