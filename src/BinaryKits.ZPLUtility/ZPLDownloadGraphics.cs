using System;
using System.Collections.Generic;
using System.Drawing;

namespace BinaryKits.Utility.ZPLUtility
{
    /// <summary>
    /// ~DGd:o.x,t,w,data
    /// </summary>
    public class ZPLDownloadGraphics : ZPLDownload
    {
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int TotalNumberOfBytes { get; set; }
        public int NumberOfBytesPerRow { get; set; }
        public Bitmap Image { get; set; }

        public ZPLDownloadGraphics(char storageDevice, string imageName, string extension, Bitmap image)
            : base(storageDevice)
        {
            ImageName = imageName;
            Extension = extension;
            Image = image;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            //Resize based on dpi
            var contextImage = new Bitmap(Image, new Size((int)Math.Round(Image.Width * context.ScaleFactor), (int)Math.Round(Image.Height * context.ScaleFactor)));

            NumberOfBytesPerRow = (int)(Math.Ceiling(contextImage.Width / 8.0)); //Each pixel is 1 bit or (1/8) byte
            TotalNumberOfBytes = contextImage.Height * NumberOfBytesPerRow;

            var result = new List<string>
            {
                $"~DG{StorageDevice}:{ImageName}.{Extension},{TotalNumberOfBytes},{NumberOfBytesPerRow},"
            };

            //Foreach row
            for (int row = 0; row < contextImage.Height; row++)
            {
                string rowBinary = string.Empty;
                for (int col = 0; col < contextImage.Width; col++)
                {
                    rowBinary += contextImage.GetPixel(col, row).GetBrightness() >= 0.5 ? "0" : "1";
                }

                //Round up to X8
                rowBinary = rowBinary.PadRight(NumberOfBytesPerRow * 8, '0');
                //Each hexadecimal character represents a horizontal nibble of four dots
                string rowHex = string.Empty;
                for (int i = 0; i < rowBinary.Length; i += 4)
                {
                    rowHex += Convert.ToInt32(rowBinary.Substring(i, 4), 2).ToString("X");
                }

                result.Add(rowHex);
            }

            return result;
        }
    }
}
