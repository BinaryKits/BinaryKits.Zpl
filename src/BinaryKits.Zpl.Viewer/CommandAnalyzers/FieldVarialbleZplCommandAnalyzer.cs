namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    // todo: fix virtual printer, must enable the MC command
    // todo: factor out common parts from FieldDataZplCommandAnalyzer so both can inherit
    // This is currently just a hack to be able to visualize single ups zpl.
    // The FV command is normally used with the MC command when printing multiple labels of a same pattern.
    // The MC command allows us to "save" the first label as a template
    // using FDs as static elements of template and FVs as variable parts.
    // Subsequent labels only require FV to draw the variable parts.
    public class FieldVariableZplCommandAnalyzer : FieldDataZplCommandAnalyzer
    {
        public FieldVariableZplCommandAnalyzer(VirtualPrinter virtualPrinter): base(virtualPrinter, "^FV") { }
    }
}
