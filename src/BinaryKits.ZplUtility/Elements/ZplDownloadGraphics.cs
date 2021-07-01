using BinaryKits.ZplUtility.Helper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// Download Graphics<br/>
    /// The ~DG command downloads an ASCII Hex representation of a graphic image.
    /// If .GRF is not the specified file extension, .GRF is automatically appended.
    /// </summary>
    /// <remarks>
    /// Format:~DGd:o.x,t,w,data
    /// d = device to store image
    /// o = image name
    /// x = extension
    /// t = total number of bytes in graphic
    /// w = number of bytes per row
    /// data = ASCII hexadecimal string defining image
    /// </remarks>
    public class ZplDownloadGraphics : ZplDownload
    {
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int TotalNumberOfBytes { get; set; }
        public int NumberOfBytesPerRow { get; set; }
        public byte[] ImageData { get; set; }
        public bool IsCompressionActive { get; set; }

        public ZplDownloadGraphics(char storageDevice, string imageName, string extension, byte[] imageData, bool isCompressionActive = true)
            : base(storageDevice)
        {
            ImageName = imageName;
            Extension = extension;
            ImageData = imageData;
            IsCompressionActive = isCompressionActive;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            byte[] objectData;
            using (var image = Image.Load(ImageData))
            {
                if (context.ScaleFactor != 1)
                {
                    var scaleWidth = (int)Math.Round(image.Width * context.ScaleFactor);
                    var scaleHeight = (int)Math.Round(image.Height * context.ScaleFactor);

                    image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                }

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    objectData = ms.ToArray();
                }
            }

            var hex = ImageHelper.ConvertBitmap(objectData, out var binaryByteCount, out var bytesPerRow);

            if (IsCompressionActive)
            {
                hex = ImageHelper.CompressHex(hex, bytesPerRow);
            }

            var result = new List<string>
            {
                $"~DG{StorageDevice}:{ImageName}.{Extension},{binaryByteCount},{bytesPerRow},"
            };

            result.Add(hex);

            return result;
        }
    }
}
