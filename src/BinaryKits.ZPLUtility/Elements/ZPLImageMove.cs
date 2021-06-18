using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    /// <summary>
    /// ^IMd:o.x
    /// </summary>
    public class ZPLImageMove : ZPLPositionedElementBase
    {
        public char StorageDevice { get; set; }
        public string ObjectName { get; set; }
        public string Extension { get; set; }

        public ZPLImageMove(int positionX, int positionY, char storageDevice, string objectName, string extension)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ObjectName = objectName;
            Extension = extension;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add($"^IM{StorageDevice}:{ObjectName}.{Extension}");

            return result;
        }
    }
}
