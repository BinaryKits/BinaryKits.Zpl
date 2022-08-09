using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Labelary;
using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.Viewer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ViewerController : ControllerBase
    {
        private readonly ILogger<ViewerController> _logger;

        public ViewerController(ILogger<ViewerController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<RenderResponseDto>> Render(RenderRequestDto request)
        {
            try
            {
                return await RenderZpl(request);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task<ActionResult<RenderResponseDto>> RenderZpl(RenderRequestDto request)
        {
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawer = new ZplElementDrawer(printerStorage);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(request.ZplData);

            var labelaryClient = new LabelaryClient();
            var labels = new List<RenderLabelDto>();
            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, request.LabelWidth, request.LabelHeight, request.PrintDensityDpmm);

                if (request.ShowLabelaryOverlay)
                {
                    var zpl = analyzeInfo.LabelInfos.Length > 1 ? new ZplEngine(labelInfo.ZplElements).ToZplString(new ZplRenderOptions() { }) : request.ZplData;
                    var labelaryImage = await labelaryClient.GetPreviewAsync(zpl, GetLabelaryDensity(request.PrintDensityDpmm), new LabelSize(request.LabelWidth, request.LabelHeight, Measure.Millimeter));

                    using (var bmpLabelary = LoadBitmap(labelaryImage))
                    using (var bmpDrawer = LoadBitmap(imageData))
                    {
                        GenerateOverlay(bmpDrawer, bmpLabelary);
                        labels.Add(new RenderLabelDto()
                        {
                            ImageBase64 = Convert.ToBase64String(SaveToByteArray(bmpDrawer, ImageFormat.Png))
                        });
                    }
                }
                else
                {
                    labels.Add(new RenderLabelDto() { ImageBase64 = Convert.ToBase64String(imageData) });
                }
            }

            var response = new RenderResponseDto
            {
                Labels = labels.ToArray(),
                NonSupportedCommands = analyzeInfo.UnknownCommands
            };

            return this.StatusCode(StatusCodes.Status200OK, response);
        }

        private static Bitmap LoadBitmap(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            return new Bitmap(ms);                
        }

        private static PrintDensity GetLabelaryDensity(int density)
        {
            switch (density)
            {
                case 6:
                    return PrintDensity.PD6dpmm;
                case 8:
                    return PrintDensity.PD8dpmm;
                case 12:
                    return PrintDensity.PD12dpmm;
                case 24:
                    return PrintDensity.PD24dpmm;
                default:
                    throw new NotSupportedException($"Print density '{density}' is not supported by labelary!");
            }
        }

        private static void GenerateOverlay(Bitmap bitmapBackground, Bitmap bitmapOverlay)
        {
            ToBlackWhite(bitmapOverlay);
            var colorMaps = new ColorMap[2];
            colorMaps[0] = new ColorMap() { OldColor = Color.White, NewColor = Color.Transparent };

            colorMaps[1] = new ColorMap() { NewColor = Color.FromArgb(128, Color.Red), OldColor = Color.Black };

            var imageAttr = new ImageAttributes();
            imageAttr.SetRemapTable(colorMaps);

            using(var g = Graphics.FromImage(bitmapBackground))
            {
                g.DrawImage(bitmapOverlay, new Rectangle(0, 0, bitmapOverlay.Width, bitmapOverlay.Height), 0, 0, bitmapOverlay.Width, bitmapOverlay.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        private static byte[] SaveToByteArray(Bitmap bitmap, ImageFormat imageFormat)
        {
            using (var msSave = new MemoryStream())
            {
            bitmap.Save(msSave, imageFormat);
            return msSave.ToArray();
            }
        }

        private static void ToBlackWhite(Bitmap image)
        {
            using(var copyImg = new Bitmap(image))
            using (Graphics gr = Graphics.FromImage(image)) // SourceImage is a Bitmap object
            {
                var gray_matrix = new float[][] {
                    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                    new float[] { 0,      0,      0,      1, 0 },
                    new float[] { 0,      0,      0,      0, 1 }
                };

                var ia = new ImageAttributes();
                ia.SetColorMatrix(new ColorMatrix(gray_matrix));
                ia.SetThreshold(0.8f); // Change this threshold as needed
                var rc = new Rectangle(0, 0, image.Width, image.Height);
                gr.DrawImage(copyImg, rc, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
            }
        }
    }
}
