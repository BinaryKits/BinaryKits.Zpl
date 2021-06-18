using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility
{
    /// <summary>
    /// ^XGd:o.x,mx,my
    /// </summary>
    public class ZPLRecallGraphic : ZPLPositionedElementBase
    {
        public char StorageDevice { get; set; }
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int MagnificationFactorX { get; set; }
        public int MagnificationFactorY { get; set; }

        public ZPLRecallGraphic(int positionX, int positionY, char storageDevice, string imageName, string extension, int magnificationFactorX = 1, int magnificationFactorY = 1)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ImageName = imageName;
            Extension = extension;
            MagnificationFactorX = magnificationFactorX;
            MagnificationFactorY = magnificationFactorY;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^XG{StorageDevice}:{ImageName}.{Extension},{MagnificationFactorX},{MagnificationFactorY},");

            return result;
        }
    }
}
