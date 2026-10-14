using BinaryKits.Zpl.Label.ImageConverters;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Label.Helpers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkiaSharp;
using System.IO;

namespace BinaryKits.Zpl.Label.UnitTest
{
    [TestClass]
    public class ImageConverterTest
    {
        [TestMethod]
        public void ConvertPackedGraphic_PreservesBitOrderAndTransparentPixels()
        {
            // Arrange
            ImageSharpImageConverter converter = new ImageSharpImageConverter();
            byte[] packedPixels = new byte[] { 0xAA, 0x55 };

            // Act
            byte[] pngImage = converter.ConvertImage(packedPixels, 1);
            using (SKBitmap bitmap = SKBitmap.Decode(pngImage))
            {
                // Assert
                Assert.AreEqual(8, bitmap.Width);
                Assert.AreEqual(2, bitmap.Height);
                for (int rowIndex = 0; rowIndex < 2; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                    {
                        SKColor actualColor = bitmap.GetPixel(columnIndex, rowIndex);
                        bool expectedBlackPixel = rowIndex % 2 == columnIndex % 2;
                        Assert.IsTrue(expectedBlackPixel ? actualColor == SKColors.Black : actualColor.Alpha == 0);
                    }
                }
            }
        }

        [TestMethod]
        public void ConvertRaster_ResetsPackingAtEachRowAndPadsPartialBytes()
        {
            // Arrange
            ImageSharpImageConverter converter = new ImageSharpImageConverter();
            using (SKBitmap bitmap = new SKBitmap(9, 2))
            {
                bitmap.Erase(SKColors.White);
                bitmap.SetPixel(0, 0, SKColors.Black);
                bitmap.SetPixel(8, 0, SKColors.Black);
                bitmap.SetPixel(1, 1, SKColors.Black);
                byte[] pngImage = EncodePng(bitmap);

                // Act
                ImageResult packedImage = converter.ConvertImage(pngImage);

                // Assert
                Assert.AreEqual(2, packedImage.BytesPerRow);
                Assert.AreEqual(4, packedImage.BinaryByteCount);
                CollectionAssert.AreEqual(new byte[] { 0x80, 0x80, 0x40, 0x00 }, packedImage.RawData);
            }
        }

        [TestMethod]
        public void ConvertRaster_PreservesTheExistingBlackPixelThreshold()
        {
            // Arrange
            ImageSharpImageConverter converter = new ImageSharpImageConverter();
            using (SKBitmap bitmap = new SKBitmap(2, 1))
            {
                bitmap.SetPixel(0, 0, new SKColor(127, 127, 127));
                bitmap.SetPixel(1, 0, new SKColor(128, 128, 128));
                byte[] pngImage = EncodePng(bitmap);

                // Act
                ImageResult packedImage = converter.ConvertImage(pngImage);

                // Assert
                CollectionAssert.AreEqual(new byte[] { 0x80 }, packedImage.RawData);
            }
        }

        [TestMethod]
        public void ConvertRaster_InvalidImage_ReportsDecodeFailure()
        {
            // Arrange
            ImageSharpImageConverter converter = new ImageSharpImageConverter();
            byte[] invalidImage = new byte[] { 1, 2, 3 };

            // Assert
            Assert.ThrowsExactly<InvalidDataException>(() =>
            {
                // Act
                converter.ConvertImage(invalidImage);
            });
        }

        [TestMethod]
        [DataRow(200, 200, 8)]
        [DataRow(200, 300, 12)]
        [DataRow(300, 200, 5)]
        public void DownloadObjects_EncodesPngWithTheExpectedScaledDimensions(int sourcePrintDpi, int targetPrintDpi, int expectedDimension)
        {
            // Arrange
            using (SKBitmap sourceBitmap = new SKBitmap(8, 8))
            {
                sourceBitmap.Erase(SKColors.Black);
                ZplDownloadObjects downloadObject = new ZplDownloadObjects('R', "TEST.PNG", EncodePng(sourceBitmap));
                ZplRenderOptions renderOptions = new ZplRenderOptions { SourcePrintDpi = sourcePrintDpi, TargetPrintDpi = targetPrintDpi };

                // Act
                string downloadCommand = downloadObject.Render(renderOptions).Single();
                string pngHex = downloadCommand.Substring(downloadCommand.LastIndexOf(',') + 1);
                using (SKBitmap renderedBitmap = SKBitmap.Decode(ByteHelper.HexToBytes(pngHex)))
                {
                    // Assert
                    Assert.AreEqual(expectedDimension, renderedBitmap.Width);
                    Assert.AreEqual(expectedDimension, renderedBitmap.Height);
                    Assert.AreEqual(SKColors.Black, renderedBitmap.GetPixel(expectedDimension / 2, expectedDimension / 2));
                }
            }
        }
        private static byte[] EncodePng(SKBitmap bitmap)
        {
            using (SKImage image = SKImage.FromBitmap(bitmap))
            using (SKData pngData = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return pngData.ToArray();
            }
        }
    }
}
