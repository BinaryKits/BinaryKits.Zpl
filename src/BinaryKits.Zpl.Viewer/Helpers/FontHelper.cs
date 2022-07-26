using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    public static class FontHelper
    {

        public static Font ToSystemDrawingFont(this SKFont skFont)
        {
            using(var fontCollection = new PrivateFontCollection())
            using(var fontStream = skFont.Typeface.OpenStream())
            {
                byte[] fontBytes = (byte[])Array.CreateInstance(typeof(byte), fontStream.Length);
                fontStream.Read(fontBytes, fontBytes.Length);
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, fontPtr, fontBytes.Length);
                fontCollection.AddMemoryFont(fontPtr, fontBytes.Length);
                Marshal.FreeCoTaskMem(fontPtr);
                return new Font(fontCollection.Families[0], skFont.Size);
            }
        }

    }
}
