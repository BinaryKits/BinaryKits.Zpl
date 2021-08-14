using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            var drawer = new ZplElementDrawer(printerStorage);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(request.ZplData);

            var labels = new List<LabelDto>();
            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                var imageData = drawer.Draw(labelInfo.ZplElements, request.LabelWidth, request.LabelHeight, request.PrintDensityDpmm);
                var label = new LabelDto
                {
                    ImageBase64 = Convert.ToBase64String(imageData)
                };
                labels.Add(label);
            }

            var response = new RenderResponseDto
            {
                Labels = labels.ToArray(),
                UnknownCommands = analyzeInfo.UnknownCommands
            };

            return this.StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
