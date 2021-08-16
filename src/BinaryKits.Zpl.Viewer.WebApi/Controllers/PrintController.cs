using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace BinaryKits.Zpl.Viewer.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PrintController : ControllerBase
    {
        private readonly ILogger<PrintController> _logger;

        public PrintController(ILogger<PrintController> logger)
        {
            this._logger = logger;
        }

        [HttpPost]
        public ActionResult<RenderResponseDto> Render(PrintRequestDto request)
        {
            // Open connection
            using var tcpClient = new TcpClient();
            tcpClient.Connect(request.PrinterIpAddress, 9100);

            // Send Zpl data to printer
            using var writer = new System.IO.StreamWriter(tcpClient.GetStream());
            writer.Write(request.ZplData);
            writer.Flush();

            // Close Connection
            writer.Close();
            tcpClient.Close();

            return this.StatusCode(StatusCodes.Status200OK);
        }
    }
}
