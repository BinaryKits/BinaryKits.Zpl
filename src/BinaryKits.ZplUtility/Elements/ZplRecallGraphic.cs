using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// ^XGd:o.x,mx,my
    /// </summary>
    public class ZplRecallGraphic : ZplPositionedElementBase
    {
        public char StorageDevice { get; set; }
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int MagnificationFactorX { get; set; }
        public int MagnificationFactorY { get; set; }

        public ZplRecallGraphic(int positionX, int positionY, char storageDevice, string imageName, string extension, int magnificationFactorX = 1, int magnificationFactorY = 1)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ImageName = imageName;
            Extension = extension;
            MagnificationFactorX = magnificationFactorX;
            MagnificationFactorY = magnificationFactorY;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^XG{StorageDevice}:{ImageName}.{Extension},{MagnificationFactorX},{MagnificationFactorY},");

            return result;
        }
    }
}
