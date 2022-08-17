using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Image Move<br/>
    /// *The* ^IM command performs a direct move of an image from storage area into the bitmap.
    /// The command is identical to the ^XG command (Recall Graphic), except there are no sizing parameters
    /// </summary>
    /// <remarks>
    /// Format:^IMd:o.x<br/>
    /// d = location of stored object<br/>
    /// o = object name<br/>
    /// x = extension<br/>
    /// </remarks>
    public class ZplImageMove : ZplPositionedElementBase
    {
        public char StorageDevice { get; private set; }
        public string ObjectName { get; private set; }
        public string Extension { get; private set; }

        /// <summary>
        /// Image Move
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="storageDevice"></param>
        /// <param name="objectName"></param>
        /// <param name="extension"></param>
        public ZplImageMove(int positionX, int positionY, char storageDevice, string objectName, string extension)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ObjectName = objectName;
            Extension = extension;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^IM{StorageDevice}:{ObjectName}.{Extension}");

            return result;
        }
    }
}
