using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldSeparatorZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        private static readonly FieldDataZplCommandAnalyzer fieldDataAnalyzer = new();

        public FieldSeparatorZplCommandAnalyzer() : base("^FS") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            // If next field number has been set and was not consumed by a field data
            // it has to be stored as a command so that it is handled when merging formats
            ZplElementBase element = null;
            int? fieldNumber = virtualPrinter.NextFieldNumber;
            if (fieldNumber.HasValue)
            {
                virtualPrinter.ClearNextFieldNumber();
                ZplElementBase dataElement = fieldDataAnalyzer.Analyze(zplCommand, virtualPrinter, printerStorage);
                element = new ZplFieldNumber(fieldNumber.Value, dataElement);
            }

            virtualPrinter.ClearNextElementPosition();
            virtualPrinter.ClearNextElementFieldBlock();
            virtualPrinter.ClearNextElementFieldData();
            virtualPrinter.ClearNextElementFieldReverse();
            virtualPrinter.ClearNextElementFieldHexadecimalIndicator();
            virtualPrinter.ClearNextElementFieldJustification();
            virtualPrinter.ClearNextFont();
            virtualPrinter.ClearComments();

            return element;
        }
    }
}
