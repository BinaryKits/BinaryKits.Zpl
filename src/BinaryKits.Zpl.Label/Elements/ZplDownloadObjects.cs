using BinaryKits.Zpl.Label.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Download Graphics / Native TrueType or OpenType Font
    /// The ~DY command downloads to the printer graphic objects or fonts in any supported format.
    /// This command can be used in place of ~DG for more saving and loading options.
    /// ~DY is the preferred command to download TrueType fonts on printers with firmware greater than X.13.
    /// It is faster than ~DU.
    /// </summary>
    /// <remarks>
    /// Format:~DYd:f,b,x,t,w,data
    /// d = file location
    /// f = file name
    /// b = format downloaded in data field
    /// x = extension of stored file
    /// t = total number of bytes in file
    /// w = total number of bytes per row
    /// data = data
    /// </remarks>
    public class ZplDownloadObjects : ZplDownload
    {
        public string ObjectName { get; private set; }
        public byte[] ImageData { get; private set; }

        public ZplDownloadObjects(char storageDevice, string imageName, byte[] imageData)
            : base(storageDevice)
        {
            ObjectName = imageName;
            ImageData = imageData;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            byte[] objectData;
            using (var image = SKBitmap.Decode(ImageData)
                ?? throw new InvalidDataException("The image data could not be decoded."))
            {
                SKBitmap imageToEncode = image;
                SKBitmap resizedImage = null;
                if (context.ScaleFactor != 1)
                {
                    var scaleWidth = (int)Math.Round(image.Width * context.ScaleFactor);
                    var scaleHeight = (int)Math.Round(image.Height * context.ScaleFactor);
                    resizedImage = image.Resize(
                        new SKSizeI(scaleWidth, scaleHeight),
                        new SKSamplingOptions(SKFilterMode.Linear))
                        ?? throw new InvalidOperationException("The image could not be resized.");
                    imageToEncode = resizedImage;
                }

                using (resizedImage)
                using (var snapshot = SKImage.FromBitmap(imageToEncode))
                using (var encoded = snapshot.Encode(SKEncodedImageFormat.Png, 100)
                    ?? throw new InvalidOperationException("The image could not be encoded as PNG."))
                {
                    objectData = encoded.ToArray();
                }
            }

            var hexString = ByteHelper.BytesToHex(objectData);

            var formatDownloadedInDataField = 'P'; //portable network graphic (.PNG) - ZB64 encoded 
            var extensionOfStoredFile = 'P'; //store as compressed (.PNG)

            var result = new List<string>
            {
                $"~DY{StorageDevice}:{ObjectName},{formatDownloadedInDataField},{extensionOfStoredFile},{objectData.Length},,{hexString}"
            };

            return result;
        }
    }
}
