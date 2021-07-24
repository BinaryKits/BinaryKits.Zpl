using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
        public ActionResult<RenderResponseDto> Render(RenderRequestDto request)
        {
            IPrinterStorage printerStorage = new PrinterStorage();

            var analyzer = new ZplAnalyzer(printerStorage);
            var elements = analyzer.Analyze(request.ZplData);

            var drawer = new ZplElementDrawer(printerStorage);
            var imageData = drawer.Draw(elements);

            var response = new RenderResponseDto
            {
                ImageBase64 = Convert.ToBase64String(imageData)
            };

            return this.StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
