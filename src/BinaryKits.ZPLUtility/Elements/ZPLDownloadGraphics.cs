using BinaryKits.ZPLUtility.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BinaryKits.Utility.ZPLUtility.Elements
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
    public class ZPLDownloadGraphics : ZPLDownload
    {
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int TotalNumberOfBytes { get; set; }
        public int NumberOfBytesPerRow { get; set; }
        public Bitmap Image { get; set; }
        public bool IsCompressionActive { get; set; }

        public ZPLDownloadGraphics(char storageDevice, string imageName, string extension, Bitmap image, bool isCompressionActive = true)
            : base(storageDevice)
        {
            ImageName = imageName;
            Extension = extension;
            Image = image;
            IsCompressionActive = isCompressionActive;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            Bitmap contextImage;
            if (context.ScaleFactor == 1)
            {
                contextImage = Image;
            }
            else
            {
                //Resize based on dpi
                contextImage = new Bitmap(Image, new Size((int)Math.Round(Image.Width * context.ScaleFactor), (int)Math.Round(Image.Height * context.ScaleFactor)));
            }

            var hex = ImageHelper.ConvertBitmapToHex(contextImage, out var binaryByteCount, out var bytesPerRow);

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
