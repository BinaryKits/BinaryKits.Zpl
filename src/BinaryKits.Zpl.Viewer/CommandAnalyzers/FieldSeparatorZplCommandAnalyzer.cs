using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldSeparatorZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private ZplCommandAnalyzerBase _fieldDataAnalyzer;

        public FieldSeparatorZplCommandAnalyzer(
            VirtualPrinter virtualPrinter, ZplCommandAnalyzerBase fieldDataAnalyzer)
            : base("^FS", virtualPrinter)
        {
            _fieldDataAnalyzer = fieldDataAnalyzer;
        }

        public override ZplElementBase Analyze(string zplCommand)
        {
            // If next field number has been set and was not consumed by a field data
            // it has to be stored as a command so that it is handled when merging formats
            ZplElementBase element = null;
            int? fieldNumber = this.VirtualPrinter.NextFieldNumber;
            if (fieldNumber.HasValue)
            {
                this.VirtualPrinter.ClearNextFieldNumber();
                ZplElementBase dataElement = _fieldDataAnalyzer.Analyze(zplCommand);
                element = new ZplFieldNumber(fieldNumber.Value, dataElement);
            }

            this.VirtualPrinter.ClearNextElementPosition();
            this.VirtualPrinter.ClearNextElementFieldBlock();
            this.VirtualPrinter.ClearNextElementFieldData();
            this.VirtualPrinter.ClearNextElementFieldReverse();
            this.VirtualPrinter.ClearNextFont();
            this.VirtualPrinter.ClearComments();

            return element;
        }
    }
}
