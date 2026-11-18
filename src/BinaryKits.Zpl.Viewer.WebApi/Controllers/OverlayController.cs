using BinaryKits.Zpl.Labelary;
using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
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

            using var image = SKBitmap.Decode(labelaryImageData)
                ?? throw new InvalidOperationException("The Labelary image could not be decoded.");
            using var filteredImage = new SKBitmap(image.Width, image.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            using var colorFilter = SKColorFilter.CreateColorMatrix(new[]
            {
                -0.299f, 0, 0, -0.1495f, 1,
                -0.587f, 0, 0, -0.2935f, 0,
                -0.114f, 0, 0, -0.0570f, 0,
                0, 0, 0, 0, 0.5f
            });
            using (var canvas = new SKCanvas(filteredImage))
            using (var paint = new SKPaint { ColorFilter = colorFilter })
            {
                canvas.DrawBitmap(image, 0, 0, paint);
                canvas.Flush();
            }

            using var snapshot = SKImage.FromBitmap(filteredImage);
            using var encoded = snapshot.Encode(SKEncodedImageFormat.Png, 100)
                ?? throw new InvalidOperationException("The Labelary image could not be encoded as PNG.");

            var response = new RenderResponseDto
            {
                Labels = new RenderLabelDto[]
                {
                    new RenderLabelDto { ImageBase64 = Convert.ToBase64String(encoded.ToArray()) }
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
