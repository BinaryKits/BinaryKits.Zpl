using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryKits.Zpl.Protocol.ImageConverters
{
    /// <summary>
    /// ImageSharp Image Converter
    /// </summary>
    public class ImageSharpImageConverter : IImageConverter
    {
        ///<inheritdoc/>
        public ImageResult ConvertImage(byte[] imageData)
        {
            var zplBuilder = new StringBuilder();

            using (Image<Rgba32> image = Image.Load(imageData))
            {
                var bytesPerRow = image.Width % 8 > 0
                    ? image.Width / 8 + 1
                    : image.Width / 8;

                var binaryByteCount = image.Height * bytesPerRow;

                var colorBits = 0;
                var j = 0;

                for (var y = 0; y < image.Height; y++)
                {
                    var row = image.GetPixelRowSpan(y);

                    for (var x = 0; x < image.Width; x++)
                    {
                        var pixel = row[x];

                        var isBlackPixel = ((pixel.R + pixel.G + pixel.B) / 3) < 128;
                        if (isBlackPixel)
                        {
                            colorBits |= 1 << (7 - j);
                        }

                        j++;

                        if (j == 8 || x == (image.Width - 1))
                        {
                            zplBuilder.Append(colorBits.ToString("X2"));
                            colorBits = 0;
                            j = 0;
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

        ///<inheritdoc/>
        public byte[] ConvertImage(byte[] imageData, int bytesPerRow)
        {
            imageData = imageData.Select(b => Reverse(b)).ToArray();

            var imageHeight = imageData.Length / bytesPerRow;
            var imageWidth = bytesPerRow * 8;

            using (var image = new Image<Rgba32>(imageWidth, imageHeight))
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var row = image.GetPixelRowSpan(y);
                    var bits = new BitArray(imageData.Skip(bytesPerRow * y).Take(bytesPerRow).ToArray());

                    for (var x = 0 ; x < image.Width; x++)
                    {
                        if (bits[x])
                        {
                            row[x].A = 255;
                        }
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    image.SaveAsPng(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
