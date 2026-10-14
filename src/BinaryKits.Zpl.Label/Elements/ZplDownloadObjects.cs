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
            using (var image = SKBitmap.Decode(ImageData))
            {
                if (image == null)
                {
                    throw new InvalidDataException("Unable to decode image data.");
                }

                if (context.ScaleFactor != 1)
                {
                    var scaleWidth = (int)Math.Round(image.Width * context.ScaleFactor);
                    var scaleHeight = (int)Math.Round(image.Height * context.ScaleFactor);

                    using (var resizedImage = new SKBitmap(scaleWidth, scaleHeight, image.ColorType, image.AlphaType))
                    {
                        image.ScalePixels(resizedImage, new SKSamplingOptions(SKCubicResampler.Mitchell));
                        objectData = EncodePng(resizedImage);
                    }
                }
                else
                {
                    objectData = EncodePng(image);
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

        private static byte[] EncodePng(SKBitmap image)
        {
            using (SKImage skImage = SKImage.FromBitmap(image))
            using (SKData data = skImage.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }
    }
}
