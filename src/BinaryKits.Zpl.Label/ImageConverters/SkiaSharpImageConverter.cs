using SkiaSharp;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace BinaryKits.Zpl.Label.ImageConverters
{
    public class SkiaSharpImageConverter : IImageConverter
    {
        /// <summary>
        /// Convert image to bitonal image (grf)
        /// </summary>
        public ImageResult ConvertImage(byte[] imageData)
        {
            using var image = Decode(imageData);
            var bytesPerRow = (image.Width + 7) / 8;
            var rawData = new byte[image.Height * bytesPerRow];

            for (var y = 0; y < image.Height; y++)
            {
                for (var x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    if ((pixel.Red + pixel.Green + pixel.Blue) / 3 < 128)
                    {
                        rawData[y * bytesPerRow + x / 8] |= (byte)(1 << (7 - x % 8));
                    }
                }
            }

            return new ImageResult
            {
                RawData = rawData,
                BinaryByteCount = rawData.Length,
                BytesPerRow = bytesPerRow
            };
        }

        /// <summary>
        /// Convert from bitonal image (grf) to png image
        /// </summary>
        public byte[] ConvertImage(byte[] imageData, int bytesPerRow)
        {
            imageData = imageData.Select(Reverse).ToArray();
            var imageHeight = imageData.Length / bytesPerRow;
            var imageWidth = bytesPerRow * 8;

            using var image = new SKBitmap(imageWidth, imageHeight, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            image.Erase(SKColors.Transparent);

            for (var y = 0; y < image.Height; y++)
            {
                var bits = new BitArray(imageData.Skip(bytesPerRow * y).Take(bytesPerRow).ToArray());
                for (var x = 0; x < image.Width; x++)
                {
                    if (bits[x])
                    {
                        image.SetPixel(x, y, SKColors.Black);
                    }
                }
            }

            using var encoded = EncodePng(image);
            return encoded.ToArray();
        }

        private static SKBitmap Decode(byte[] imageData)
        {
            return SKBitmap.Decode(imageData)
                ?? throw new InvalidDataException("The image data could not be decoded.");
        }

        private static SKData EncodePng(SKBitmap image)
        {
            using var snapshot = SKImage.FromBitmap(image);
            return snapshot.Encode(SKEncodedImageFormat.Png, 100)
                ?? throw new InvalidOperationException("The image could not be encoded as PNG.");
        }

        private static byte Reverse(byte value)
        {
            var reversed = 0;
            for (var i = 0; i < 8; i++)
            {
                if ((value & (1 << i)) != 0)
                {
                    reversed |= 1 << (7 - i);
                }
            }

            return (byte)reversed;
        }
    }
}
