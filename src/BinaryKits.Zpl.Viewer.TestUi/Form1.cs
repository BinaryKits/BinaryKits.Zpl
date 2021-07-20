using BinaryKits.Zpl.Viewer;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BinaryKits.ZplRenderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();

            var files = Directory.GetFiles("ZplDatas");
            this.comboBoxFile.DataSource = files;

            this.AnalyzeAndDraw();
        }

        private void AnalyzeAndDraw()
        {
            IPrinterStorage printerStorage = new PrinterStorage();

            var analyzer = new ZplAnalyzer(printerStorage);
            var elements = analyzer.Analyze(this.textBoxZplData.Text);

            var drawer = new ZplElementDrawer(printerStorage);
            var imageData = drawer.Draw(elements);

            var oldImage = this.pictureBox1.Image;
            this.pictureBox1.Image = this.GetBitmap(imageData);
            oldImage?.Dispose();
        }

        private Bitmap GetBitmap(byte[] imageData)
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Write(imageData, 0, imageData.Length);
            return new Bitmap(memoryStream);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.AnalyzeAndDraw();
        }

        private void textBoxZplData_TextChanged(object sender, EventArgs e)
        {
            this.AnalyzeAndDraw();
        }

        private void comboBoxFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxZplData.Text = File.ReadAllText(this.comboBoxFile.SelectedItem.ToString());
        }
    }
}
