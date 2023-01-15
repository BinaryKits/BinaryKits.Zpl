namespace BinaryKits.Zpl.Viewer.WebApi.Models
{
    public class RenderRequestDto
    {
        /// <summary>
        /// Zpl data
        /// </summary>
        public string ZplData { get; set; }
        /// <summary>
        /// Label width in Millimeter
        /// </summary>
        public double LabelWidth { get; set; } = 101.6;
        /// <summary>
        /// Label height in Millimeter
        /// </summary>
        public double LabelHeight { get; set; } = 152.4;
        /// <summary>
        /// Dots per Millimeter
        /// </summary>
        public int PrintDensityDpmm { get; set; } = 8;
    }
}
