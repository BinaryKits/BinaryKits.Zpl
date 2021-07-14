using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Label
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
            public static readonly ZplFont Default = new ZplFont(30, 30, "0", FieldOrientation.Normal);
        }
    }
}
