using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Text;

namespace BinaryKits.Zpl.Label.ImageConverters
{
    public class ImageSharpImageConverter : IImageConverter
    {
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

        public byte[] ConvertImage(byte[] imageData, int bytesPerRow)
        {
            var imageWidth = imageData.Length / bytesPerRow;
            var imageHeight = bytesPerRow * 8;

            using (var image = new Image<Rgba32>(imageWidth, imageHeight))
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var row = image.GetPixelRowSpan(y);

                    for (var x = 0; x < image.Width; x++)
                    {
                        var index = x * y;
                        if (index >= imageData.Length)
                        {
                            continue;
                        }
                        var imageByte = imageData[index];

                        //TODO: fix, does not work yet
                        if (imageByte > 0)
                        {
                            row[x].A = 255;
                            row[x].R = 0;
                            row[x].G = 0;
                            row[x].B = 0;
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
