using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public interface IZplCommandAnalyzer
    {
        bool CanAnalyze(string zplLine);

        ZplElementBase Analyze(string zplCommand);
    }
}
