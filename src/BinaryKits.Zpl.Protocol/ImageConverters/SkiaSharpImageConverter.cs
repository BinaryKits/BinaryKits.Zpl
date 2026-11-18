using SkiaSharp;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Protocol.ImageConverters
{
    /// <summary>
    /// SkiaSharp image converter
    /// </summary>
    public class SkiaSharpImageConverter : IImageConverter
    {
        ///<inheritdoc/>
        public ImageResult ConvertImage(byte[] imageData)
        {
            var zplBuilder = new StringBuilder();

            using (var image = Decode(imageData))
            {
                var bytesPerRow = (image.Width + 7) / 8;
                var binaryByteCount = image.Height * bytesPerRow;

                for (var y = 0; y < image.Height; y++)
                {
                    var colorBits = 0;
                    var bitIndex = 0;

                    for (var x = 0; x < image.Width; x++)
                    {
                        var pixel = image.GetPixel(x, y);
                        if ((pixel.Red + pixel.Green + pixel.Blue) / 3 < 128)
                        {
                            colorBits |= 1 << (7 - bitIndex);
                        }

                        bitIndex++;
                        if (bitIndex == 8 || x == image.Width - 1)
                        {
                            zplBuilder.Append(colorBits.ToString("X2"));
                            colorBits = 0;
                            bitIndex = 0;
                        }
                    }

                    zplBuilder.Append('\n');
                }

                return new ImageResult
                {
                    ZplData = zplBuilder.ToString(),
                    BinaryByteCount = binaryByteCount,
                    BytesPerRow = bytesPerRow
                };
            }
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

        ///<inheritdoc/>
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
    }
}
