using BinaryKits.Zpl.Viewer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace BinaryKits.Zpl.Viewer.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        [HttpGet]
        public ActionResult<LabelResponseDto> Get()
        {
            var items = new List<LabelItemDto>();

            items.AddRange(this.GetLabelsFromFolder("Example"));
            items.AddRange(this.GetLabelsFromFolder("Test"));

            var response = new LabelResponseDto
            {
                Items = items.ToArray()
            };

            return StatusCode(StatusCodes.Status200OK, response);
        }

        private LabelItemDto[] GetLabelsFromFolder(string folder)
        {
            var path = Path.Combine("Labels", folder);

            var items = new List<LabelItemDto>();

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var nameParts = fileName.Split('-');
                var name = nameParts[0];
                var format = string.Empty;
                if (nameParts.Length > 1)
                {
                    format = nameParts[1];
                }

                var content = System.IO.File.ReadAllText(file);

                items.Add(new LabelItemDto { Name = name, Category = folder, Format = format, Content = content });
            }

            return items.ToArray();
        }
    }
}
