using BinaryKits.ZplUtility.Helper;
using BinaryKits.ZplUtility.ImageConverter;
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

        private bool _isCompressionActive;
        private IImageConverter _imageConverter;

        public ZplDownloadGraphics(
            char storageDevice,
            string imageName,
            string extension,
            byte[] imageData,
            bool isCompressionActive = true,
            IImageConverter imageConverter = default)
            : base(storageDevice)
        {
            ImageName = imageName;
            Extension = extension;
            ImageData = imageData;

            if (imageConverter == default)
            {
                imageConverter = new ImageSharpImageConverter();
            }

            _isCompressionActive = isCompressionActive;
            _imageConverter = imageConverter;
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

            var imageResult = _imageConverter.ConvertImage(objectData);

            if (_isCompressionActive)
            {
                imageResult.ZplData = CompressHelper.CompressHex(imageResult.ZplData, imageResult.BytesPerRow);
            }

            return new List<string>
            {
                $"~DG{StorageDevice}:{ImageName}.{Extension},{imageResult.BinaryByteCount},{imageResult.BytesPerRow},",
                imageResult.ZplData
            };
        }
    }
}
