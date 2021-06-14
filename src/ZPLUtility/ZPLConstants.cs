namespace BinaryKits.Utility.ZPLUtility
{
    public static class ZPLConstants
    {
        public static class InternationalFontEncoding
        {
            //Unicode (UTF-8 encoding) - Unicode Character Set
            public static readonly string CI28 = "^CI28";
            //13 = Zebra Code Page 850 (see page 1194)
            public static readonly string CI13 = "^CI13";
        }

        public static class Orientation
        {
            public static readonly string Normal = "N";
            //R = rotated 90 degrees(clockwise)
            public static readonly string Rotated90 = "R";
            //I = inverted 180 degrees
            public static readonly string Rotated180 = "I";
            //B = read from bottom up, 270 degrees
            public static readonly string Rotated270 = "B";
        }

        public static class Font
        {
            public static readonly ZPLFont Default = new ZPLFont(30, 30, "0", "N");
        }

        //^FB
        public static class TextJustification
        {
            //L = left
            public static readonly string Left = "L";
            //C = center
            public static readonly string Center = "C";
            //R = right
            public static readonly string Right = "R";
            //J = justified
            public static readonly string Justified = "J";
        }

        public static class LineColor
        {
            public static readonly string Black = "B";
            public static readonly string White = "W";
        }
    }
}
