using BinaryKits.Zpl.Label.Helpers;
using BinaryKits.Zpl.Label.ImageConverters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryKits.Zpl.Label.Elements
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
        public string ImageName { get; private set; }
        private string _extension { get; set; }
        public byte[] ImageData { get; private set; }

        private readonly IImageConverter _imageConverter;
        readonly ZplCompressionScheme _compressionScheme;

        /// <summary>
        /// Zpl Download Graphics
        /// </summary>
        /// <param name="storageDevice"></param>
        /// <param name="imageName"></param>
        /// <param name="imageData"></param>
        /// <param name="imageConverter"></param>
        /// <param name="compressionScheme"></param>
        public ZplDownloadGraphics(
            char storageDevice,
            string imageName,
            byte[] imageData,
            ZplCompressionScheme compressionScheme = ZplCompressionScheme.ACS,
            IImageConverter imageConverter = default)
            : base(storageDevice)
        {
            if (imageName.Length > 8)
            {
                new ArgumentException("maximum length of 8 characters exceeded", nameof(imageName));
            }

            _extension = "GRF"; //Fixed

            ImageName = imageName;
            ImageData = imageData;

            if (imageConverter == default)
            {
                imageConverter = new ImageSharpImageConverter();
            }
            _imageConverter = imageConverter;
            _compressionScheme = compressionScheme;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            byte[] objectData;
            using (var image = Image.Load(ImageData))
            {
                if (context.ScaleFactor != 1)
                {
                    //var scaleWidth = (int)Math.Round(image.Width * context.ScaleFactor);
                    //var scaleHeight = (int)Math.Round(image.Height * context.ScaleFactor);

                    image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                }

                using (var ms = new MemoryStream())
                {
                    //ImageSharp v3 workaround. 
#if NET6_0_OR_GREATER
                    PngMetadata metadata = image.Metadata.GetPngMetadata();
                    metadata.ColorTable = null;
#endif
                    image.Save(ms, new PngEncoder());
                    objectData = ms.ToArray();
                }
            }

            var imageResult = _imageConverter.ConvertImage(objectData);
            string zplData = string.Empty;

            switch (_compressionScheme)
            {
                case ZplCompressionScheme.None:
                    zplData = imageResult.RawData.ToHexFromBytes();
                    break;
                case ZplCompressionScheme.ACS:
                    zplData = ZebraACSCompressionHelper.Compress(imageResult.RawData.ToHexFromBytes(), imageResult.BytesPerRow);
                    break;
                case ZplCompressionScheme.Z64:
                    //TODO: Reduce multiple conversions of byte array to string. 
                    zplData = ZebraZ64CompressionHelper.Compress(imageResult.RawData);
                    break;
                case ZplCompressionScheme.B64:
                    //TODO: Implement this compression scheme.
                    zplData = ZebraB64CompressionHelper.Compress(imageResult.RawData);
                    break;
                    //throw new NotSupportedException();
            }

            return new List<string>
            {
                $"~DG{StorageDevice}:{ImageName}.{_extension},{imageResult.BinaryByteCount},{imageResult.BytesPerRow},",
                zplData
            };
        }
    }
}
