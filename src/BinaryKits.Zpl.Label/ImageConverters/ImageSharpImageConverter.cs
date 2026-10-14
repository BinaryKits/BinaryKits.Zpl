using SkiaSharp;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace BinaryKits.Zpl.Label.ImageConverters
{
    public class ImageSharpImageConverter : IImageConverter
    {
        /// <summary>
        /// Convert image to bitonal image (grf)
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public ImageResult ConvertImage(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData.Length))
            {
                using (SKBitmap image = DecodeImage(imageData))
                {
                    if (image == null)
                    {
                        throw new InvalidDataException("Unable to decode image data.");
                    }

                    var bytesPerRow = image.Width % 8 > 0
                        ? image.Width / 8 + 1
                        : image.Width / 8;

                    var binaryByteCount = image.Height * bytesPerRow;

                    var colorBits = 0;
                    var j = 0;

                    for (var y = 0; y < image.Height; y++)
                    {
                        for (var x = 0; x < image.Width; x++)
                        {
                            var pixel = image.GetPixel(x, y);

                            var isBlackPixel = ((pixel.Red + pixel.Green + pixel.Blue) / 3) < 128;
                            if (isBlackPixel)
                            {
                                colorBits |= 1 << (7 - j);
                            }

                            j++;

                            if (j == 8 || x == (image.Width - 1))
                            {
                                ms.WriteByte((byte)colorBits);
                                colorBits = 0;
                                j = 0;
                            }
                        }
                    }

                    return new ImageResult
                    {
                        RawData = ms.ToArray(),
                        BinaryByteCount = binaryByteCount,
                        BytesPerRow = bytesPerRow
                    };
                }
            }


        }

        private byte Reverse(byte b)
        {
            var reverse = 0;
            for (var i = 0; i < 8; i++)
            {
                if ((b & (1 << i)) != 0)
                {
                    reverse |= 1 << (7 - i);
                }
            }
            return (byte)reverse;
        }

        /// <summary>
        /// Convert from bitonal image (grf) to png image
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="bytesPerRow"></param>
        /// <returns></returns>
        public byte[] ConvertImage(byte[] imageData, int bytesPerRow)
        {
            imageData = imageData.Select(b => Reverse(b)).ToArray();

            var imageHeight = imageData.Length / bytesPerRow;
            var imageWidth = bytesPerRow * 8;

            using (var image = new SKBitmap(imageWidth, imageHeight, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
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

                return EncodePng(image);
            }
        }

        private static SKBitmap DecodeImage(byte[] imageData)
        {
            try
            {
                return SKBitmap.Decode(imageData);
            }
            catch (ArgumentException exception)
            {
                throw new InvalidDataException("Unable to decode image data.", exception);
            }
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
