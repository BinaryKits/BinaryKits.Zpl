namespace BinaryKits.Zpl.Viewer.Models
{
    public class AnalyzeInfo
    {
        public string[] Errors { get; set; }

        public string[] UnknownCommands { get; set; }

        public LabelInfo[] LabelInfos { get; set; }
    }
}
