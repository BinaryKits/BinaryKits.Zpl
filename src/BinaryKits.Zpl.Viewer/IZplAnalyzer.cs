using BinaryKits.Zpl.Viewer.Models;

namespace Application.UseCase.ZplToPdf
{
    public interface IZplAnalyzer
    {
        public AnalyzeInfo Analyze(string zplData);
    }
}
