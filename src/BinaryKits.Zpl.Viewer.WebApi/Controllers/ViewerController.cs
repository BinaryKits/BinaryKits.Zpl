using BinaryKits.Zpl.Viewer.ElementDrawers;
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
            try
            {
                return RenderZpl(request);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private ActionResult<RenderResponseDto> RenderZpl(RenderRequestDto request)
        {
            IPrinterStorage printerStorage = new PrinterStorage();
            var drawerOptions = new DrawerOptions();
            drawerOptions.OpaqueBackground = true; //set white background for viewer requests

            //PDF mode (image mode is default)
            if (request.Type == "PDF")
            {
                drawerOptions.PdfOutput = true;
            }

            var drawer = new ZplElementDrawer(printerStorage, drawerOptions);

            var analyzer = new ZplAnalyzer(printerStorage);
            var analyzeInfo = analyzer.Analyze(request.ZplData);

            var labels = new List<RenderLabelDto>();
            var pdfs = new List<RenderLabelDto>();
            foreach (var labelInfo in analyzeInfo.LabelInfos)
            {
                if (request.Type == "image")
                {
                    var imageData = drawer.Draw(labelInfo.ZplElements, request.LabelWidth, request.LabelHeight, request.PrintDensityDpmm);
                    var label = new RenderLabelDto
                    {
                        ImageBase64 = Convert.ToBase64String(imageData)
                    };
                    labels.Add(label);
                }
                
                if (request.Type == "PDF")
                {
                    var pdfData = drawer.DrawPdf(labelInfo.ZplElements, request.LabelWidth, request.LabelHeight, request.PrintDensityDpmm);
                    var pdf = new RenderLabelDto
                    {
                        PdfBase64 = Convert.ToBase64String(pdfData)
                    };
                    pdfs.Add(pdf);
                }
                
                if (request.Type == "both")
                {
                    var bothData = drawer.DrawMulti(labelInfo.ZplElements, request.LabelWidth, request.LabelHeight, request.PrintDensityDpmm);
                    
                    var imageData = bothData[0];
                    var label = new RenderLabelDto
                    {
                        ImageBase64 = Convert.ToBase64String(imageData)
                    };
                    labels.Add(label);
                    
                    var pdfData = bothData[1];
                    var pdf = new RenderLabelDto
                    {
                        PdfBase64 = Convert.ToBase64String(pdfData)
                    };
                    pdfs.Add(pdf);
                }
            }

            var response = new RenderResponseDto
            {
                Labels = labels.ToArray(),
                Pdfs = pdfs.ToArray(),
                NonSupportedCommands = analyzeInfo.UnknownCommands
            };

            return this.StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
