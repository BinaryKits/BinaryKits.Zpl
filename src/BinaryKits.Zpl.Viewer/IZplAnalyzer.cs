using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer
{
    public interface IZplAnalyzer
    {
        AnalyzeInfo Analyze(string zplData);
    }
}
