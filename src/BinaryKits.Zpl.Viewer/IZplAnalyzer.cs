using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer
{
    public interface IZplAnalyzer
    {
        public AnalyzeInfo Analyze(string zplData);
    }
}
