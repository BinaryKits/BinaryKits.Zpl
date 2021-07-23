using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace BinaryKits.Zpl.Viewer.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestDataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<TestDataResponseDto> Get()
        {
            var items = new List<ZplTestDataDto>();

            var files = Directory.GetFiles("ZplDatas");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var content = System.IO.File.ReadAllText(file);

                items.Add(new ZplTestDataDto { Name = fileInfo.Name, Content = content });
            }

            return StatusCode(StatusCodes.Status200OK, new TestDataResponseDto { Items = items.ToArray() });
        }
    }
}
