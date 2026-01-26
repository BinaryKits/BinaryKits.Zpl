using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplQrCode : ZplFieldDataElementBase
    {
        public int Model { get; private set; }
        public int MagnificationFactor { get; private set; }
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; private set; }
        public int MaskValue { get; private set; }
        public int VerticalQuietZone { get; private set; }

        /// <summary>
        /// Zpl QrCode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="model">1 (original) and 2 (enhanced – recommended)</param>
        /// <param name="magnificationFactor">Size of the QR code, 1 on 150 dpi printers, 2 on 200 dpi printers, 3 on 300 dpi printers, 6 on 600 dpi printers</param>
        /// <param name="errorCorrectionLevel"></param>
        /// <param name="maskValue">0-7, (default: 7)</param>
        /// <param name="fieldOrientation"></param>
        /// <param name="hexadecimalIndicator"></param> 
        /// <param name="bottomToTop"></param>
        /// <param name="useDefaultPosition"></param>
        /// <param name="verticalQuietZone">The vertical quiet zone (usually set with <c>^BY,,X</c>)</param>
        public ZplQrCode(
            string content,
            int positionX,
            int positionY,
            int model = 2,
            int magnificationFactor = 2,
            ErrorCorrectionLevel errorCorrectionLevel = ErrorCorrectionLevel.HighReliability,
            int maskValue = 7,
            FieldOrientation fieldOrientation = FieldOrientation.Normal,
            char? hexadecimalIndicator = null,
            bool bottomToTop = false,
            bool useDefaultPosition = false,
            int verticalQuietZone = 10)
            : base(content, positionX, positionY, fieldOrientation, hexadecimalIndicator, bottomToTop, useDefaultPosition)
        {
            Model = model;
            MagnificationFactor = magnificationFactor;
            ErrorCorrectionLevel = errorCorrectionLevel;
            MaskValue = maskValue;
            VerticalQuietZone = verticalQuietZone;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO100,100
            //^ BQN,2,10
            //^ FDMM,AAC - 42 ^ FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BQ{RenderFieldOrientation()},{Model},{context.Scale(MagnificationFactor)},{RenderErrorCorrectionLevel(ErrorCorrectionLevel)},{MaskValue}");
            result.Add(RenderFieldDataSection());

            return result;
        }

        protected new string RenderFieldDataSection()
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

            sb.Append($"^FD{RenderErrorCorrectionLevel(ErrorCorrectionLevel)}A,{Content}^FS");

            return sb.ToString();
        }
    }
}
