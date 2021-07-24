namespace BinaryKits.Zpl.Viewer
{
    public class ZplCommandStructure
    {
        public string PreviousCommand { get; set; }
        public string CurrentCommand { get; set; }
        public string NextCommand { get; set; }

        public ZplCommandStructure()
        { }

        public ZplCommandStructure(string currentCommand)
        {
            this.CurrentCommand = currentCommand;
        }
    }
}
