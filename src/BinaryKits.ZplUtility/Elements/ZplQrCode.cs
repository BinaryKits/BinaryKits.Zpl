using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    public class ZplQrCode : ZplPositionedElementBase
    {
        public string Content { get; protected set; }

        public int Model { get; set; }

        public int MagnificationFactor { get; set; }

        public string ErrorCorrection { get; set; }
        public int MaskValue { get; set; }

        public ZplQrCode(string content, int positionX, int positionY, int model = 2, int magnificationFactor = 2, string errorCorrection = "Q", int maskValue = 7) : base(positionX, positionY)
        {
            Content = content;
            Model = model;
            MagnificationFactor = magnificationFactor;
            ErrorCorrection = errorCorrection;
            MaskValue = maskValue;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^ FO100,100
            //^ BQN,2,10
            //^ FDMM,AAC - 42 ^ FS
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^BQN,{Model},{context.Scale(MagnificationFactor)},{ErrorCorrection},{MaskValue}");
            result.Add($"^FD{ErrorCorrection}A,{Content}^FS");

            return result;
        }
    }
}
