using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using CoelWu.Zint.Net;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class MaxiCodeElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplMaxiCode;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplMaxiCode maxiCode)
            {
                var maxiBarcode = new ZintNetLib();
                maxiBarcode.MaxicodeMode = ConvertMaxiCodeMode(maxiCode.Mode);
                var content = maxiCode.Content;
                
                try
                {
                    //replace hex items before mode 2 and mode 3 specific string manipulation 
                    if (maxiCode.UseHexadecimalIndicator)
                    {
                        content = content.ReplaceHexEscapes();
                    }
                    
                    if (maxiBarcode.MaxicodeMode == MaxicodeMode.Mode2)
                    {
                        //ZPL mode 2:
                        //^FD[aaa][bbb][ccccc][dddd]_5B)>_1E[ee]_1D[ff]1Z
                        //^FD002840100450000_5B)>_1E01_1D961Z
                        //<hpm> = aaabbbcccccdddd
                        // aaa = three-digit class of service
                        // bbb = three-digit country code
                        // ccccc = five-digit zip code
                        // dddd = four-digit zip code extension (if none exists, four zeros (0000) must be entered)
                        // ee = 01
                        // ff = year number
                        //ZintNetLib structure:
                        //^FD_5B)>_1E[ee]_1D[ff][ccccc][dddd]_1D[bbb]_1D[aaa]_1D1Z
                        //^FD_5B)>_1E01_1D96100450000_1D840_1D002_1D1Z
                        //Note: Country code 630 is not allowed in this library for mode 2 

                        var replace = content.Substring(0, 24);
                        var aaa = content.Substring(0, 3);
                        var bbb = content.Substring(3, 3);
                        var ccccc = content.Substring(6, 5);
                        var dddd = content.Substring(11, 4);
                        var ee = content.Substring(19, 2);
                        var ff = content.Substring(22, 2);
                        var newString = $"\x5B)>\x1E{ee}\x1D{ff}{ccccc}{dddd}\x1D{bbb}\x1D{aaa}\x1D";
                        content = content.Replace(replace, newString);
                    }
                    else if (maxiBarcode.MaxicodeMode == MaxicodeMode.Mode3)
                    {
                        //ZPL mode 3:
                        //^FD[aaa][bbb][cccccc]_5B)>_1E[ee]_1D[ff]1Z
                        //^FD066826RS19  _5B)>_1E01_1D961Z
                        //<hpm> = aaabbbcccccc
                        // aaa = three-digit class of service
                        // bbb = three-digit country code
                        // ccccc = six-digit zip code (A through Z or 0 to 9)
                        // ee = 01
                        // ff = year number
                        //ZintNetLib structure:
                        //^FD_5B)>_1E[ee]_1D[ff][cccccc]_1D[bbb]_1D[aaa]_1D1Z
                        //^FD_5B)>_1E01_1D96RS19  _1D826_1D066_1D1Z
                        
                        var replace = content.Substring(0, 21);
                        var aaa = content.Substring(0, 3);
                        var bbb = content.Substring(3, 3);
                        var cccccc = content.Substring(6, 6);
                        var ee = content.Substring(16, 2);
                        var ff = content.Substring(19, 2);
                        var newString = $"\x5B)>\x1E{ee}\x1D{ff}{cccccc}\x1D{bbb}\x1D{aaa}\x1D";
                        content = content.Replace(replace, newString);
                    }
                    
                    maxiBarcode.CreateBarcode("Maxicode(ISO 16023)", content);
                }
                catch (Exception e)
                {
                    // Do nothing
                }

                //^BD2,1,1
                if (maxiBarcode.IsValid)
                {
                    var bitmap = new Bitmap(1000, 1000);
                    var graphics = Graphics.FromImage(bitmap);
                    graphics.Clear(Color.White);

                    maxiBarcode.Rotation = 0;
                    maxiBarcode.Multiplier = 2;
                    //Note: Position and Total are not supported in this MaxiCode library
                    maxiBarcode.DrawBarcode(graphics, new Point(-6, -6));
                    
                    //Linux container fix, redraw the circles (variation of: CoelWu.Zint.Net.ZintNetLib.DrawBarcode)
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    float num3 = 13.64f * 7f;
                    float num4 = 13.43f * 7f;
                    float num5 = 0.85f * 7f;
                    float num6 = 2.2f * 7f;
                    float num7 = 3.54f * 7f;
                    Pen pen = new Pen(Color.Black, 0.67f * 7f);
                    graphics.DrawEllipse(pen, new RectangleF(num3 - num5 + 1, num4 - num5 + 1, num5 * 2.12f, num5 * 2.12f));
                    graphics.DrawEllipse(pen, new RectangleF(num3 - num6 + 0.5f, num4 - num6 + 0.5f, num6 * 2.12f, num6 * 2.12f));
                    graphics.DrawEllipse(pen, new RectangleF(num3 - num7, num4 - num7, num7 * 2.12f, num7 * 2.12f));

                    Size symbolSize;
                    var section = Rectangle.Empty;
                    symbolSize = maxiBarcode.SymbolSize(graphics);
                    section.Width = symbolSize.Width - 12;
                    section.Height = symbolSize.Height - 12;

                    var newBitmap = new Bitmap(section.Width, section.Height);
                    var newGraphics = Graphics.FromImage(newBitmap);
                    newGraphics.DrawImage(bitmap, 0, 0, section, GraphicsUnit.Pixel);

                    byte[] data = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        newBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        data = ms.ToArray();

                        this.DrawBarcode(
                            data,
                            section.Width,
                            section.Height,
                            true,
                            maxiCode.PositionX,
                            maxiCode.PositionY,
                            0,
                            Label.FieldOrientation.Normal
                        );
                    }
                }
            }
        }

        private MaxicodeMode ConvertMaxiCodeMode(int mode)
        {
            return mode switch {
                2 => MaxicodeMode.Mode2,
                3 => MaxicodeMode.Mode3,
                4 => MaxicodeMode.Mode4,
                5 => MaxicodeMode.Mode5,
                6 => MaxicodeMode.Mode6,
                _ => MaxicodeMode.Mode2,
            };
        }
    }
}
