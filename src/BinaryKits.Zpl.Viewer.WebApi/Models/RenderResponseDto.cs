namespace BinaryKits.Zpl.Viewer.WebApi.Models
{
    public class RenderResponseDto
    {
        public string[] NonSupportedCommands { get; set; }
        public RenderLabelDto[] Labels { get; set; }
    }
}
