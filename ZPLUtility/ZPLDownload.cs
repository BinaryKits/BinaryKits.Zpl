using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace BinaryKits.Utility.ZPLUtility
{
    public abstract class ZPLDownload : ZPLElementBase
    {
        /// <summary>
        /// R, E, B, and A
        /// </summary>
        public char StorageDevice { get; set; }

        public ZPLDownload(char storageDevice)
        {
            StorageDevice = storageDevice;
        }
    }

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

            List<string> result = new List<string>
            {
                string.Format("~DG{0}:{1}.{2},{3},{4},", StorageDevice, ImageName, Extension, TotalNumberOfBytes, NumberOfBytesPerRow)
            };

            //Foreach row
            for (int row = 0; row < contextImage.Height; row++)
            {
                string rowBinary = "";
                for (int col = 0; col < contextImage.Width; col++)
                {
                    rowBinary += contextImage.GetPixel(col, row).GetBrightness() >= 0.5 ? "0" : "1";
                }

                //Round up to X8
                rowBinary = rowBinary.PadRight(NumberOfBytesPerRow * 8, '0');
                //Each hexadecimal character represents a horizontal nibble of four dots
                string rowHex = "";
                for (int i = 0; i < rowBinary.Length; i += 4)
                {
                    rowHex += Convert.ToInt32(rowBinary.Substring(i, 4), 2).ToString("X");
                }

                result.Add(rowHex);
            }

            return result;
        }
    }

    /// <summary>
    /// ~DYd:f,b,x,t,w,data
    /// </summary>
    public class ZPLDownloadObjects : ZPLDownload
    {
        public string ObjectName { get; set; }
        public Bitmap Image { get; set; }

        public ZPLDownloadObjects(char storageDevice, string imageName, Bitmap image)
            : base(storageDevice)
        {
            ObjectName = imageName;
            Image = image;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            var contextImage = new Bitmap(Image, new Size((int)Math.Round(Image.Width * context.ScaleFactor), (int)Math.Round(Image.Height * context.ScaleFactor)));

            byte[] objectData;
            using (var ms = new MemoryStream())
            {
                contextImage.Save(ms, ImageFormat.Png);
                objectData = ms.ToArray();
            }

            StringBuilder sb = new StringBuilder();
            foreach (Byte b in objectData)
            {
                sb.Append(String.Format("{0:X}", b).PadLeft(2, '0'));
            }
            string dataString = sb.ToString();

            List<string> result = new List<string>
            {
                string.Format("~DY{0}:{1}{2},{3},{4},{5},{6},{7}", StorageDevice, ObjectName,"", "P", "P", objectData.Length, "", dataString)
            };

            return result;
        }
    }

    /// <summary>
    /// ^IMd:o.x
    /// </summary>
    public class ZPLImageMove : ZPLPositionedElementBase
    {
        public char StorageDevice { get; set; }
        public string ObjectName { get; set; }
        public string Extension { get; set; }

        public ZPLImageMove(int positionX, int positionY, char storageDevice, string objectName, string extension)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ObjectName = objectName;
            Extension = extension;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add(string.Format("^IM{0}:{1}.{2}", StorageDevice, ObjectName, Extension));

            return result;
        }
    }

    /// <summary>
    /// ^XGd:o.x,mx,my
    /// </summary>
    public class ZPLRecallGraphic : ZPLPositionedElementBase
    {
        public char StorageDevice { get; set; }
        public string ImageName { get; set; }
        public string Extension { get; set; }
        public int MagnificationFactorX { get; set; }
        public int MagnificationFactorY { get; set; }

        public ZPLRecallGraphic(int positionX, int positionY, char storageDevice, string imageName, string extension, int magnificationFactorX = 1, int magnificationFactorY = 1)
            : base(positionX, positionY)
        {
            StorageDevice = storageDevice;
            ImageName = imageName;
            Extension = extension;
            MagnificationFactorX = magnificationFactorX;
            MagnificationFactorY = magnificationFactorY;
        }

        public override IEnumerable<string> Render(ZPLRenderOptions context)
        {
            List<string> result = new List<string>();
            result.AddRange(Origin.Render(context));
            result.Add(string.Format("^XG{0}:{1}.{2},{3},{4},", StorageDevice, ImageName, Extension, MagnificationFactorX, MagnificationFactorY));

            return result;
        }
    }
}
