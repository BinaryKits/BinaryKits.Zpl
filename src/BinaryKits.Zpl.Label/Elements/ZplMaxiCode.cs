using System.Collections.Generic;
using System.Text;

namespace BinaryKits.Zpl.Label.Elements
{
    public class ZplMaxiCode : ZplPositionedElementBase, IFormatElement
    {
        public string Content { get; protected set; }

        public int Mode { get; private set; }

        public int Position { get; private set; }

        public int Total { get; private set; }
        
        public bool UseHexadecimalIndicator { get; protected set; }

        /// <summary>
        /// Zpl QrCode
        /// </summary>
        /// <param name="content"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="mode">2 (numeric postal code) Default, 3 (alphanumeric postal code), 4 (standard), 5 (full EEC), and 6 (reader programming)</param>
        /// <param name="position">1-8, (default: 1)</param>
        /// <param name="total">1-8, (default: 1)</param>
        /// <param name="useHexadecimalIndicator"></param>
        /// <param name="bottomToTop"></param>
        public ZplMaxiCode(
            string content,
            int positionX,
            int positionY,
            int mode = 2,
            int position = 1,
            int total = 1,
            bool useHexadecimalIndicator = false,
            bool bottomToTop = false)
            : base(positionX, positionY, bottomToTop)
        {
            Content = content;
            Mode = mode;
            Position = position;
            Total = total;
            UseHexadecimalIndicator = useHexadecimalIndicator;
        }

        ///<inheritdoc/>
        public override IEnumerable<string> Render(ZplRenderOptions context)
        {
            //^FO100,100
            //^BD2,1,1
            //^FH^FD002840100450000_5B)>_1E01_1D961Z00136071_1DUPSN_1D123X56_1D028_1D_1D001/001_1D011_1DN_1D_1DNEW YORK_1DNY_1E_04^FS
            var result = new List<string>();
            result.AddRange(RenderPosition(context));
            result.Add($"^BD{Mode},{Position},{Total}");
            result.Add(RenderFieldDataSection());

            return result;
        }
        
        protected string RenderFieldDataSection()
        {
            var sb = new StringBuilder();
            if (UseHexadecimalIndicator)
            {
                sb.Append("^FH");
            }

            if (Content != null)
            {
                sb.Append("^FD");
                sb.Append(Content);
                sb.Append("^FS");
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public void SetTemplateContent(string content)
        {
            Content = content;
        }
    }
}
