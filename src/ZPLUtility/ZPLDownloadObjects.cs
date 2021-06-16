using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace BinaryKits.Utility.ZPLUtility
{
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
            byte[] objectData;
            using (var contextImage = new Bitmap(Image, new Size((int)Math.Round(Image.Width * context.ScaleFactor), (int)Math.Round(Image.Height * context.ScaleFactor))))
            {
                using (var ms = new MemoryStream())
                {
                    contextImage.Save(ms, ImageFormat.Png);
                    objectData = ms.ToArray();
                }
            }

            var sb = new StringBuilder();
            foreach (byte b in objectData)
            {
                sb.Append(string.Format("{0:X}", b).PadLeft(2, '0'));
            }

            var result = new List<string>
            {
                $"~DY{StorageDevice}:{ObjectName},P,P,{objectData.Length},,{sb}"
            };

            return result;
        }
    }
}
