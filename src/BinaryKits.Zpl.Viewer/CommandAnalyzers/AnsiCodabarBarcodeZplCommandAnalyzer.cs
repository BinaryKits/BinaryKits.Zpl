using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers;

public class AnsiCodabarBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
{
    public AnsiCodabarBarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^BK", virtualPrinter) { }

    public override ZplElementBase Analyze(string zplCommand)
    {
        var zplDataParts = this.SplitCommand(zplCommand);
        
        bool checkDigit = false;
        int height = this.VirtualPrinter.BarcodeInfo.Height;
        bool printInterpretationLine = true;
        bool printInterpretationLineAboveCode = false;
        char startCharacter = 'A';
        char stopCharacter = 'A';

        var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
        
        if (zplDataParts.Length > 1)
        {
            checkDigit = this.ConvertBoolean(zplDataParts[1], "Y");
        }

        if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], out int tmpint))
        {
            height = tmpint;
        }
        
        if (zplDataParts.Length > 3)
        {
            printInterpretationLine = this.ConvertBoolean(zplDataParts[3], "Y");
        }

        if (zplDataParts.Length > 4)
        {
            printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[4]);
        }
        
        if (zplDataParts.Length > 5 && char.TryParse(zplDataParts[6], out char startChar))
        {
           startCharacter = startChar;
        }
        
        if (zplDataParts.Length > 6 && char.TryParse(zplDataParts[6], out char stopChar))
        {
           stopCharacter = stopChar;
        }

        //The field data are processing in the FieldDataZplCommandAnalyzer
        this.VirtualPrinter.SetNextElementFieldData(new AnsiCodabarFieldData
        {
            FieldOrientation = fieldOrientation,
            StartCharacter = startCharacter,
            StopCharacter = stopCharacter,
            Height = height,
            CheckDigit = checkDigit,
            PrintInterpretationLine = printInterpretationLine,
            PrintInterpretationLineAboveCode = printInterpretationLineAboveCode,
        });

        return null;
    }
}
