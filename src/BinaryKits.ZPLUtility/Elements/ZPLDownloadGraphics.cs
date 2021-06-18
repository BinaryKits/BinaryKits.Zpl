using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility.Elements
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
            var sw = new Stopwatch();
            sw.Start();

            Bitmap contextImage;
            if (context.ScaleFactor == 1)
            {
                contextImage = Image;
            }
            else
            {
                //Resize based on dpi
                contextImage = new Bitmap(Image, new Size((int)Math.Round(Image.Width * context.ScaleFactor), (int)Math.Round(Image.Height * context.ScaleFactor)));
            }

            //Each pixel is 1 bit or (1/8) byte
            var bitsPerByte = 8;

            NumberOfBytesPerRow = (int)(Math.Ceiling(contextImage.Width / (double)bitsPerByte));
            TotalNumberOfBytes = contextImage.Height * NumberOfBytesPerRow;
            var requiredRowLenght = NumberOfBytesPerRow * bitsPerByte;

            var result = new List<string>
            {
                $"~DG{StorageDevice}:{ImageName}.{Extension},{TotalNumberOfBytes},{NumberOfBytesPerRow},"
            };

            var rowBinary = new BitArray(requiredRowLenght);
            var sb = new StringBuilder();

            //Foreach row
            for (int row = 0; row < contextImage.Height; row++)
            {
                for (var col = 0; col < contextImage.Width; col++)
                {
                    rowBinary.Set(col, contextImage.GetPixel(col, row).GetBrightness() >= 0.5 ? false : true);
                }

                //Each hexadecimal character represents a horizontal nibble of four dots
                for (var i = 0; i < rowBinary.Length; i += 4)
                {
                    var hex = GetIntFromBitArray(CopySlice(rowBinary, i, 4)).ToString("X");
                    sb.Append(hex);
                }

                result.Add(sb.ToString());
                sb.Clear();
            }

            sw.Stop();
            Debug.WriteLine(sw.Elapsed.TotalMilliseconds);

            return result;
        }

        private BitArray CopySlice(BitArray source, int offset, int length)
        {
            var ret = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                ret[i] = source[offset + length - 1 - i];
            }
            return ret;
        }

        private int GetIntFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length > 32)
            {
                throw new ArgumentException("Argument length shall be at most 32 bits.");
            }

            var array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];
        }
    }
}
