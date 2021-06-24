using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace BinaryKits.ZplUtility.Elements
{
    /// <summary>
    /// Download Graphics / Native TrueType or OpenType Font
    /// The ~DY command downloads to the printer graphic objects or fonts in any supported format.
    /// This command can be used in place of ~DG for more saving and loading options.
    /// ~DY is the preferred command to download TrueType fonts on printers with firmware greater than X.13.
    /// It is faster than ~DU.
    /// </summary>
    /// <remarks>
    /// Format:~DYd:f,b,x,t,w,data
    /// d = file location
    /// f = file name
    /// b = format downloaded in data field
    /// x = extension of stored file
    /// t = total number of bytes in file
    /// w = total number of bytes per row
    /// data = data
    /// </remarks>
    public class ZplDownloadObjects : ZplDownload
    {
        public string ObjectName { get; set; }
        public Bitmap Image { get; set; }

        public ZplDownloadObjects(char storageDevice, string imageName, Bitmap image)
            : base(storageDevice)
        {
            ObjectName = imageName;
            Image = image;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
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

            var formatDownloadedInDataField = 'P'; //portable network graphic (.PNG) - ZB64 encoded 
            var extensionOfStoredFile = 'P'; //store as compressed (.PNG)

            var result = new List<string>
            {
                $"~DY{StorageDevice}:{ObjectName},{formatDownloadedInDataField},{extensionOfStoredFile},{objectData.Length},,{sb}"
            };

            return result;
        }
    }
}
