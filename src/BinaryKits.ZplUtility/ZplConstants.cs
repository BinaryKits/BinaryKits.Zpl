using BinaryKits.ZplUtility.Elements;

namespace BinaryKits.ZplUtility
{
    public static class ZplConstants
    {
        public static class InternationalFontEncoding
        {
            /// <summary>
            /// Unicode (UTF-8 encoding) - Unicode Character Set
            /// </summary>
            public static readonly string CI28 = "^CI28";
            /// <summary>
            /// 13 = Zebra Code Page 850 (see page 1194)
            /// </summary>
            public static readonly string CI13 = "^CI13";
        }

        public static class Font
        {
            public static readonly ZplFont Default = new ZplFont(30, 30, "0", "N");
        }

        //^FB
        public static class TextJustification
        {
            /// <summary>
            /// Left
            /// </summary>
            public static readonly string Left = "L";
            /// <summary>
            /// Center
            /// </summary>
            public static readonly string Center = "C";
            /// <summary>
            /// Right
            /// </summary>
            public static readonly string Right = "R";
            /// <summary>
            /// justified
            /// </summary>
            public static readonly string Justified = "J";
        }

        public static class LineColor
        {
            /// <summary>
            /// Black
            /// </summary>
            public static readonly string Black = "B";
            /// <summary>
            /// White
            /// </summary>
            public static readonly string White = "W";
        }
    }
}
