using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Field number element proving value to a <see cref="ZplFieldNumber"/>.
    /// </summary>
    public class ZplRecallFieldNumber : ZplElementBase
    {
        public int Number { get; private set; }
        public string Text { get; private set; }

        public ZplRecallFieldNumber(int number, string text)
        {
            Number = number;
            Text = text;
        }

        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("^FD");
            foreach (var c in Text)
            {
                sb.Append(ZplTextField.SanitizeCharacter(c));
            }

            var result = new List<string>();
            result.Add($"^FN{Number}");
            result.Add($"{sb}^FS");
            return result;
        }
    }
}
