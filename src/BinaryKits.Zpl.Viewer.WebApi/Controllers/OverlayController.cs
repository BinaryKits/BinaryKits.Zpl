using BinaryKits.Zpl.Labelary;
using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BinaryKits.Zpl.Viewer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OverlayController : ControllerBase
    {
        private readonly ILogger<OverlayController> _logger;

        public OverlayController(ILogger<OverlayController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<RenderResponseDto>> Render(RenderRequestDto request)
        {
            try
            {
                return await RenderOverlay(request);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task<ActionResult<RenderResponseDto>> RenderOverlay(RenderRequestDto request)
        {
            var labelaryClient = new LabelaryClient();
            var labelaryImageData = await labelaryClient.GetPreviewAsync(
                request.ZplData,
                ConvertLabelaryDensity(request.PrintDensityDpmm),
                new LabelSize(request.LabelWidth, request.LabelHeight, Measure.Millimeter)
            );

            using var image = Image.Load(labelaryImageData).CloneAs<Rgba32>();
            ColorMatrix colorMatrix = new ColorMatrix(
                -0.299f, 0, 0, -0.1495f,
                -0.587f, 0, 0, -0.2935f,
                -0.114f, 0, 0, -0.0570f,
                 0,      0, 0,  0,
                 1,      0, 0,  0.5f
            );
            image.Mutate(ctx => ctx.Filter(colorMatrix));

            using var stream = new MemoryStream();
            image.Save(stream, new PngEncoder { ColorType = PngColorType.RgbWithAlpha });

            var response = new RenderResponseDto
            {
                Labels = new RenderLabelDto[]
                {
                    new RenderLabelDto { ImageBase64 = Convert.ToBase64String(stream.ToArray()) }
                }
            };

            return this.StatusCode(StatusCodes.Status200OK, response);
        }

        private static PrintDensity ConvertLabelaryDensity(int density)
        {
            return density switch
            {
                6 => PrintDensity.PD6dpmm,
                8 => PrintDensity.PD8dpmm,
                12 => PrintDensity.PD12dpmm,
                24 => PrintDensity.PD24dpmm,
                _ => throw new NotSupportedException($"Print density '{density}' is not supported by labelary."),
            };
        }
    }
}
