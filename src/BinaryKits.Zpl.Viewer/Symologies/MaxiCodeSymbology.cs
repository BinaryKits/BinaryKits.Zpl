using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BinaryKits.Zpl.Viewer.Symologies
{
    public static class MaxiCodeSymbology
    {
        private static readonly Regex mode2Regex = new Regex(@"^(?<preamble>)(?<service>\d{3})(?<country>\d{3})(?<zip>\d{1,9})", RegexOptions.Compiled);
        private static readonly Regex mode2GsRegex = new Regex(@"^(?<preamble>\[\)>\x1e01\x1d(?<year>\d\d))(?<zip>\d{1,9})\x1d(?<country>\d{3})\x1d(?<service>\d{3})\x1d", RegexOptions.Compiled);
        private static readonly Regex mode3Regex = new Regex(@"^(?<preamble>)(?<service>\d{3})(?<country>\d{3})(?<zip>[ 0-9A-Z]{1,6})", RegexOptions.Compiled);
        private static readonly Regex mode3GsRegex = new Regex(@"^(?<preamble>\[\)>\x1e01\x1d(?<year>\d\d))(?<zip>[ 0-9A-Z]{1,6})\x1d(?<country>\d{3})\x1d(?<service>\d{3})\x1d", RegexOptions.Compiled);

        private static readonly Regex numericShiftRegex = new Regex(@"^\d{9}", RegexOptions.Compiled);
        private static readonly Regex codeARegex = new Regex(@"^[\r\x1c-\x1e ""-:A-Z]+", RegexOptions.Compiled);
        private static readonly Regex codeBRegex = new Regex(@"^[\x1c-\x1e !,./:-@\[-\x7f]+", RegexOptions.Compiled);
        //private static readonly Regex codeCRegex = new Regex(@"^[\x1c-\x1e \x80-\x89\xaa\xac\xb1-\xb3\xb5\xb9\xba\xbc-\xbe\xc0-\xdf]+", RegexOptions.Compiled);
        //private static readonly Regex codeDRegex = new Regex(@"^[\x1c-\x1e \x8a-\x94\xa1\xa8\xab\xaf\xb0\xb4\xb7\xb8\xbb\xbf\xe0-\xff]+", RegexOptions.Compiled);
        //private static readonly Regex codeERegex = new Regex(@"^[\x00- \x95-\xa0\xa2-\xa7\xa9\xad\xae\xb6]+", RegexOptions.Compiled);

        private static readonly int[] ec10Poly = [1, 31, 28, 39, 42, 57, 2, 3, 49, 44, 46];
        private static readonly int[] ec20Poly = [1, 23, 44, 11, 33, 27, 8, 22, 37, 57, 36, 15, 48, 22, 17, 38, 33, 31, 19, 23, 59];
        private static readonly int[] ec28Poly = [1, 22, 45, 53, 10, 41, 55, 35, 10, 22, 29, 23, 13, 61, 45, 34, 55, 40, 37, 46, 49, 34, 41, 9, 43, 7, 20, 11, 28];

        private enum MaxiCodeCodeSet
        {
            CodeSetA,
            CodeSetB,
            CodeSetC,
            CodeSetD,
            CodeSetE
        }

        private enum CodewordType
        {
            Right = 0,
            Left = 1,
            Down = 2,
            Irregular = 3
        }

        private struct Codeword
        {
            public CodewordType Type;
            public int Msb;
            public int[] Offsets;
        };

        private static readonly Dictionary<CodewordType, int[]> offsets = new Dictionary<CodewordType, int[]> {
            { CodewordType.Right, [0, -1, 30, 29, 59, 58] },
            { CodewordType.Left, [0, -1, 29, 28, 59, 58] },
            { CodewordType.Down, [0, 30, 29, 59, 89, 88] }
        };

        private static readonly int[] orientationPatterns = [
            // mode mark
            28, 29,
            // top left
            276, 277, 306,
            // middle left
            450, 480,
            // middle right
            492, 522,
            // bottom left
            659, 689,
            // bottom right
            666, 696
        ];

        private static readonly Codeword[] codewords = [
            // primary message
            // 1-9
            new Codeword { Type = CodewordType.Irregular, Msb = 462, Offsets = [0, 59, -180, -151, -120, -121] },
            new Codeword { Type = CodewordType.Irregular, Msb = 662, Offsets = [0, -1, 30, 29, -25, 3] },
            new Codeword { Type = CodewordType.Irregular, Msb = 279, Offsets = [0, -1, 29, 28, 85, 321] },
            new Codeword { Type = CodewordType.Irregular, Msb = 608, Offsets = [0, -235, -236, -205, -206, -176] },
            new Codeword { Type = CodewordType.Irregular, Msb = 694, Offsets = [0, -1, -144, -114, -115, -85] },
            new Codeword { Type = CodewordType.Irregular, Msb = 451, Offsets = [0, 59, 179, 239, 213, 212] },
            new Codeword { Type = CodewordType.Irregular, Msb = 281, Offsets = [0, -1, 29, 28, 24, 54] },
            new Codeword { Type = CodewordType.Irregular, Msb = 523, Offsets = [0, -238, -239, -209, -179, -180] },
            new Codeword { Type = CodewordType.Irregular, Msb = 449, Offsets = [0, 29, 60, 59, 15, 14] },
            // 10-20
            new Codeword { Type = CodewordType.Right, Msb = 363 },
            new Codeword { Type = CodewordType.Right, Msb = 540 },
            new Codeword { Type = CodewordType.Left, Msb = 639 },
            new Codeword { Type = CodewordType.Left, Msb = 629 },
            new Codeword { Type = CodewordType.Left, Msb = 275 },
            new Codeword { Type = CodewordType.Right, Msb = 375 },
            new Codeword { Type = CodewordType.Right, Msb = 552 },
            new Codeword { Type = CodewordType.Right, Msb = 538 },
            new Codeword { Type = CodewordType.Right, Msb = 361 },
            new Codeword { Type = CodewordType.Left, Msb = 287 },
            new Codeword { Type = CodewordType.Left, Msb = 641 },
            // secondary message
            // 21-34
            new Codeword { Type = CodewordType.Right, Msb = 1 },
            new Codeword { Type = CodewordType.Right, Msb = 3 },
            new Codeword { Type = CodewordType.Right, Msb = 5 },
            new Codeword { Type = CodewordType.Right, Msb = 7 },
            new Codeword { Type = CodewordType.Right, Msb = 9 },
            new Codeword { Type = CodewordType.Right, Msb = 11 },
            new Codeword { Type = CodewordType.Right, Msb = 13 },
            new Codeword { Type = CodewordType.Right, Msb = 15 },
            new Codeword { Type = CodewordType.Right, Msb = 17 },
            new Codeword { Type = CodewordType.Right, Msb = 19 },
            new Codeword { Type = CodewordType.Right, Msb = 21 },
            new Codeword { Type = CodewordType.Right, Msb = 23 },
            new Codeword { Type = CodewordType.Right, Msb = 25 },
            new Codeword { Type = CodewordType.Right, Msb = 27 },
            // 35-48
            new Codeword { Type = CodewordType.Left, Msb = 116 },
            new Codeword { Type = CodewordType.Left, Msb = 114 },
            new Codeword { Type = CodewordType.Left, Msb = 112 },
            new Codeword { Type = CodewordType.Left, Msb = 110 },
            new Codeword { Type = CodewordType.Left, Msb = 108 },
            new Codeword { Type = CodewordType.Left, Msb = 106 },
            new Codeword { Type = CodewordType.Left, Msb = 104 },
            new Codeword { Type = CodewordType.Left, Msb = 102 },
            new Codeword { Type = CodewordType.Left, Msb = 100 },
            new Codeword { Type = CodewordType.Left, Msb = 98 },
            new Codeword { Type = CodewordType.Left, Msb = 96 },
            new Codeword { Type = CodewordType.Left, Msb = 94 },
            new Codeword { Type = CodewordType.Left, Msb = 92 },
            new Codeword { Type = CodewordType.Left, Msb = 90 },
            // 49-62
            new Codeword { Type = CodewordType.Right, Msb = 178 },
            new Codeword { Type = CodewordType.Right, Msb = 180 },
            new Codeword { Type = CodewordType.Right, Msb = 182 },
            new Codeword { Type = CodewordType.Right, Msb = 184 },
            new Codeword { Type = CodewordType.Right, Msb = 186 },
            new Codeword { Type = CodewordType.Right, Msb = 188 },
            new Codeword { Type = CodewordType.Right, Msb = 190 },
            new Codeword { Type = CodewordType.Right, Msb = 192 },
            new Codeword { Type = CodewordType.Right, Msb = 194 },
            new Codeword { Type = CodewordType.Right, Msb = 196 },
            new Codeword { Type = CodewordType.Right, Msb = 198 },
            new Codeword { Type = CodewordType.Right, Msb = 200 },
            new Codeword { Type = CodewordType.Right, Msb = 202 },
            new Codeword { Type = CodewordType.Right, Msb = 204 },
            // 63-69
            new Codeword { Type = CodewordType.Left, Msb = 293 },
            new Codeword { Type = CodewordType.Left, Msb = 291 },
            new Codeword { Type = CodewordType.Left, Msb = 289 },
            new Codeword { Type = CodewordType.Left, Msb = 273 },
            new Codeword { Type = CodewordType.Left, Msb = 271 },
            new Codeword { Type = CodewordType.Left, Msb = 269 },
            new Codeword { Type = CodewordType.Left, Msb = 267 },
            // 70-75
            new Codeword { Type = CodewordType.Right, Msb = 355 },
            new Codeword { Type = CodewordType.Right, Msb = 357 },
            new Codeword { Type = CodewordType.Right, Msb = 359 },
            new Codeword { Type = CodewordType.Right, Msb = 377 },
            new Codeword { Type = CodewordType.Right, Msb = 379 },
            new Codeword { Type = CodewordType.Right, Msb = 381 },
            // 76-81
            new Codeword { Type = CodewordType.Left, Msb = 470 },
            new Codeword { Type = CodewordType.Left, Msb = 468 },
            new Codeword { Type = CodewordType.Left, Msb = 466 },
            new Codeword { Type = CodewordType.Left, Msb = 448 },
            new Codeword { Type = CodewordType.Left, Msb = 446 },
            new Codeword { Type = CodewordType.Left, Msb = 444 },
            // 82-87
            new Codeword { Type = CodewordType.Right, Msb = 532 },
            new Codeword { Type = CodewordType.Right, Msb = 534 },
            new Codeword { Type = CodewordType.Right, Msb = 536 },
            new Codeword { Type = CodewordType.Right, Msb = 554 },
            new Codeword { Type = CodewordType.Right, Msb = 556 },
            new Codeword { Type = CodewordType.Right, Msb = 558 },
            // 88-94
            new Codeword { Type = CodewordType.Left, Msb = 647 },
            new Codeword { Type = CodewordType.Left, Msb = 645 },
            new Codeword { Type = CodewordType.Left, Msb = 643 },
            new Codeword { Type = CodewordType.Left, Msb = 627 },
            new Codeword { Type = CodewordType.Left, Msb = 625 },
            new Codeword { Type = CodewordType.Left, Msb = 623 },
            new Codeword { Type = CodewordType.Left, Msb = 621 },
            // 95-108
            new Codeword { Type = CodewordType.Right, Msb = 709 },
            new Codeword { Type = CodewordType.Right, Msb = 711 },
            new Codeword { Type = CodewordType.Right, Msb = 713 },
            new Codeword { Type = CodewordType.Right, Msb = 715 },
            new Codeword { Type = CodewordType.Right, Msb = 717 },
            new Codeword { Type = CodewordType.Right, Msb = 719 },
            new Codeword { Type = CodewordType.Right, Msb = 721 },
            new Codeword { Type = CodewordType.Right, Msb = 723 },
            new Codeword { Type = CodewordType.Right, Msb = 725 },
            new Codeword { Type = CodewordType.Right, Msb = 727 },
            new Codeword { Type = CodewordType.Right, Msb = 729 },
            new Codeword { Type = CodewordType.Right, Msb = 731 },
            new Codeword { Type = CodewordType.Right, Msb = 733 },
            new Codeword { Type = CodewordType.Right, Msb = 735 },
            // 109-122
            new Codeword { Type = CodewordType.Left, Msb = 824 },
            new Codeword { Type = CodewordType.Left, Msb = 822 },
            new Codeword { Type = CodewordType.Left, Msb = 820 },
            new Codeword { Type = CodewordType.Left, Msb = 818 },
            new Codeword { Type = CodewordType.Left, Msb = 816 },
            new Codeword { Type = CodewordType.Left, Msb = 814 },
            new Codeword { Type = CodewordType.Left, Msb = 812 },
            new Codeword { Type = CodewordType.Left, Msb = 810 },
            new Codeword { Type = CodewordType.Left, Msb = 808 },
            new Codeword { Type = CodewordType.Left, Msb = 806 },
            new Codeword { Type = CodewordType.Left, Msb = 804 },
            new Codeword { Type = CodewordType.Left, Msb = 802 },
            new Codeword { Type = CodewordType.Left, Msb = 800 },
            new Codeword { Type = CodewordType.Left, Msb = 798 },
            // 123-136
            new Codeword { Type = CodewordType.Right, Msb = 886 },
            new Codeword { Type = CodewordType.Right, Msb = 888 },
            new Codeword { Type = CodewordType.Right, Msb = 890 },
            new Codeword { Type = CodewordType.Right, Msb = 892 },
            new Codeword { Type = CodewordType.Right, Msb = 894 },
            new Codeword { Type = CodewordType.Right, Msb = 896 },
            new Codeword { Type = CodewordType.Right, Msb = 898 },
            new Codeword { Type = CodewordType.Right, Msb = 900 },
            new Codeword { Type = CodewordType.Right, Msb = 902 },
            new Codeword { Type = CodewordType.Right, Msb = 904 },
            new Codeword { Type = CodewordType.Right, Msb = 906 },
            new Codeword { Type = CodewordType.Right, Msb = 908 },
            new Codeword { Type = CodewordType.Right, Msb = 910 },
            new Codeword { Type = CodewordType.Right, Msb = 912 },
            // 137-144
            new Codeword { Type = CodewordType.Down, Msb = 58 },
            new Codeword { Type = CodewordType.Down, Msb = 176 },
            new Codeword { Type = CodewordType.Down, Msb = 294 },
            new Codeword { Type = CodewordType.Down, Msb = 412 },
            new Codeword { Type = CodewordType.Down, Msb = 530 },
            new Codeword { Type = CodewordType.Down, Msb = 648 },
            new Codeword { Type = CodewordType.Down, Msb = 766 },
            new Codeword { Type = CodewordType.Down, Msb = 884 },
        ];

        private const string ECI = "ECI";
        private const string NS = "NS";
        private const string PAD = "PAD";
        private const string PAD1 = "PAD1";
        private const string PAD2 = "PAD2";

        private const string SHIFT_A = "SHIFT_A";
        private const string SHIFT_B = "SHIFT_B";
        private const string SHIFT_C = "SHIFT_C";
        private const string SHIFT_D = "SHIFT_D";
        private const string SHIFT_E = "SHIFT_E";

        private const string SHIFT_2A = "SHIFT_2A";
        private const string SHIFT_3A = "SHIFT_3A";

        private const string LATCH_A = "LATCH_A";
        private const string LATCH_B = "LATCH_B";

        private const string LOCK_IN_C = "LOCK_IN_C";
        private const string LOCK_IN_D = "LOCK_IN_D";
        private const string LOCK_IN_E = "LOCK_IN_E";

        private static readonly Dictionary<string, int> codeAMap;
        private static readonly Dictionary<string, int> codeBMap;
        private static readonly Dictionary<string, int> codeCMap;
        private static readonly Dictionary<string, int> codeDMap;
        private static readonly Dictionary<string, int> codeEMap;

        static MaxiCodeSymbology()
        {
            var codeSetTable = new[]
            {
                new { Value = 0, A = "\x0d", B = "`", C = "\xc0", D = "\xe0", E = "\x00" },
                new { Value = 1, A = "A", B = "a", C = "\xc1", D = "\xe1", E = "\x01" },
                new { Value = 2, A = "B", B = "b", C = "\xc2", D = "\xe2", E = "\x02" },
                new { Value = 3, A = "C", B = "c", C = "\xc3", D = "\xe3", E = "\x03" },
                new { Value = 4, A = "D", B = "d", C = "\xc4", D = "\xe4", E = "\x04" },
                new { Value = 5, A = "E", B = "e", C = "\xc5", D = "\xe5", E = "\x05" },
                new { Value = 6, A = "F", B = "f", C = "\xc6", D = "\xe6", E = "\x06" },
                new { Value = 7, A = "G", B = "g", C = "\xc7", D = "\xe7", E = "\x07" },
                new { Value = 8, A = "H", B = "h", C = "\xc8", D = "\xe8", E = "\x08" },
                new { Value = 9, A = "I", B = "i", C = "\xc9", D = "\xe9", E = "\x09" },
                new { Value = 10, A = "J", B = "j", C = "\xca", D = "\xea", E = "\x0a" },
                new { Value = 11, A = "K", B = "k", C = "\xcb", D = "\xeb", E = "\x0b" },
                new { Value = 12, A = "L", B = "l", C = "\xcc", D = "\xec", E = "\x0c" },
                new { Value = 13, A = "M", B = "m", C = "\xcd", D = "\xed", E = "\x0d" },
                new { Value = 14, A = "N", B = "n", C = "\xce", D = "\xee", E = "\x0e" },
                new { Value = 15, A = "O", B = "o", C = "\xcf", D = "\xef", E = "\x0f" },
                new { Value = 16, A = "P", B = "p", C = "\xd0", D = "\xf0", E = "\x10" },
                new { Value = 17, A = "Q", B = "q", C = "\xd1", D = "\xf1", E = "\x11" },
                new { Value = 18, A = "R", B = "r", C = "\xd2", D = "\xf2", E = "\x12" },
                new { Value = 19, A = "S", B = "s", C = "\xd3", D = "\xf3", E = "\x13" },
                new { Value = 20, A = "T", B = "t", C = "\xd4", D = "\xf4", E = "\x14" },
                new { Value = 21, A = "U", B = "u", C = "\xd5", D = "\xf5", E = "\x15" },
                new { Value = 22, A = "V", B = "v", C = "\xd6", D = "\xf6", E = "\x16" },
                new { Value = 23, A = "W", B = "w", C = "\xd7", D = "\xf7", E = "\x17" },
                new { Value = 24, A = "X", B = "x", C = "\xd8", D = "\xf8", E = "\x18" },
                new { Value = 25, A = "Y", B = "y", C = "\xd9", D = "\xf9", E = "\x19" },
                new { Value = 26, A = "Z", B = "z", C = "\xda", D = "\xfa", E = "\x1a" },
                new { Value = 27, A = ECI, B = ECI, C = ECI, D = ECI, E = ECI },
                new { Value = 28, A = "\x1c", B = "\x1c", C = "\x1c", D = "\x1c", E = PAD },
                new { Value = 29, A = "\x1d", B = "\x1d", C = "\x1d", D = "\x1d", E = PAD1 },
                new { Value = 30, A = "\x1e", B = "\x1e", C = "\x1e", D = "\x1e", E = "\x1b" },
                new { Value = 31, A = NS, B = NS, C = NS, D = NS, E = NS },
                new { Value = 32, A = " ", B = "{", C = "\xdb", D = "\xfb", E = "\x1c" },
                new { Value = 33, A = PAD, B = PAD, C = "\xdc", D = "\xfc", E = "\x1d" },
                new { Value = 34, A = "\"", B = "}", C = "\xdd", D = "\xfd", E = "\x1e" },
                new { Value = 35, A = "#", B = "~", C = "\xde", D = "\xfe", E = "\x1f" },
                new { Value = 36, A = "$", B = "\x7f", C = "\xdf", D = "\xff", E = "\x9f" },
                new { Value = 37, A = "%", B = ";", C = "\xaa", D = "\xa1", E = "\xa0" },
                new { Value = 38, A = "&", B = "<", C = "\xac", D = "\xa8", E = "\xa2" },
                new { Value = 39, A = "'", B = "=", C = "\xb1", D = "\xab", E = "\xa3" },
                new { Value = 40, A = "(", B = ">", C = "\xb2", D = "\xaf", E = "\xa4" },
                new { Value = 41, A = ")", B = "?", C = "\xb3", D = "\xb0", E = "\xa5" },
                new { Value = 42, A = "*", B = "[", C = "\xb5", D = "\xb4", E = "\xa6" },
                new { Value = 43, A = "+", B = "\\", C = "\xb9", D = "\xb7", E = "\xa7" },
                new { Value = 44, A = ",", B = "]", C = "\xba", D = "\xb8", E = "\xa9" },
                new { Value = 45, A = "-", B = "^", C = "\xbc", D = "\xbb", E = "\xad" },
                new { Value = 46, A = ".", B = "_", C = "\xbd", D = "\xbf", E = "\xae" },
                new { Value = 47, A = "/", B = " ", C = "\xbe", D = "\x8a", E = "\xb6" },
                new { Value = 48, A = "0", B = ",", C = "\x80", D = "\x8b", E = "\x95" },
                new { Value = 49, A = "1", B = ".", C = "\x81", D = "\x8c", E = "\x96" },
                new { Value = 50, A = "2", B = "/", C = "\x82", D = "\x8d", E = "\x97" },
                new { Value = 51, A = "3", B = ":", C = "\x83", D = "\x8e", E = "\x98" },
                new { Value = 52, A = "4", B = "@", C = "\x84", D = "\x8f", E = "\x99" },
                new { Value = 53, A = "5", B = "!", C = "\x85", D = "\x90", E = "\x9a" },
                new { Value = 54, A = "6", B = "|", C = "\x86", D = "\x91", E = "\x9b" },
                new { Value = 55, A = "7", B = PAD1, C = "\x87", D = "\x92", E = "\x9c" },
                new { Value = 56, A = "8", B = SHIFT_2A, C = "\x88", D = "\x93", E = "\x9d" },
                new { Value = 57, A = "9", B = SHIFT_3A, C = "\x89", D = "\x94", E = "\x9e" },
                new { Value = 58, A = ":", B = PAD2, C = LATCH_A, D = LATCH_A, E = LATCH_A },
                new { Value = 59, A = SHIFT_B, B = SHIFT_A, C = " ", D = " ", E = " " },
                new { Value = 60, A = SHIFT_C, B = SHIFT_C, C = LOCK_IN_C, D = SHIFT_C, E = SHIFT_C },
                new { Value = 61, A = SHIFT_D, B = SHIFT_D, C = SHIFT_D, D = LOCK_IN_D, E = SHIFT_D },
                new { Value = 62, A = SHIFT_E, B = SHIFT_E, C = SHIFT_E, D = SHIFT_E, E = LOCK_IN_E },
                new { Value = 63, A = LATCH_B, B = LATCH_A, C = LATCH_B, D = LATCH_B, E = LATCH_B },
            };

            codeAMap = codeSetTable.ToDictionary(item => item.A, item => item.Value);
            codeBMap = codeSetTable.ToDictionary(item => item.B, item => item.Value);
            codeCMap = codeSetTable.ToDictionary(item => item.C, item => item.Value);
            codeDMap = codeSetTable.ToDictionary(item => item.D, item => item.Value);
            codeEMap = codeSetTable.ToDictionary(item => item.E, item => item.Value);
        }

        public static bool[] Encode(string content, int mode)
        {
            bool[] result = new bool[974];
            foreach (int idx in orientationPatterns)
            {
                result[idx] = true;
            }

            List<int> data = Analyze(content, mode);

            foreach ((int val, int idx) in data.Select((v, i) => (v, i)))
            {
                Codeword codeword = codewords[idx];
                int[] offs = codeword.Offsets;
                if (codeword.Type != CodewordType.Irregular)
                {
                    offs = offsets[codeword.Type];
                }

                for (int i = 0; i < 6; i++)
                {
                    result[codeword.Msb + offs[i]] = (val & (1 << 5 - i)) > 0;
                }
            }

            return result;
        }


        private static List<int> Analyze(string content, int mode)
        {
            List<int> data = new List<int>();
            bool eec = false;
            MaxiCodeCodeSet codeSet = MaxiCodeCodeSet.CodeSetA;
            var codeMap = codeAMap;

            if (mode == 2)
            {
                Match mode2Match = mode2Regex.Match(content);

                if (!mode2Match.Success)
                {
                    mode2Match = mode2GsRegex.Match(content);
                }

                if (!mode2Match.Success)
                {
                    // invalid mode 2 data, convert to mode 4
                    return Analyze(content, 4);
                }
  
                int service = int.Parse(mode2Match.Groups["service"].Value);
                int country = int.Parse(mode2Match.Groups["country"].Value);
                string zip = mode2Match.Groups["zip"].Value;
                if (country == 840)
                {
                    zip = zip.PadRight(9, '0');
                }

                int ziplen = zip.Length;
                int zipval = int.Parse(zip);


                // ISO/IEC 12023:2000 pp. 26
                data.Add(((zipval & 3) << 4) | mode); // 1-6

                data.Add((zipval >> 2) & 63);  // 7-12
                data.Add((zipval >> 8) & 63); //  13-18
                data.Add((zipval >> 14) & 63); // 19-24
                data.Add((zipval >> 20) & 63); // 25-30

                data.Add(((ziplen & 3) << 4) | (zipval >> 26)); // 31-36
                data.Add(((country & 3) << 4) | (ziplen >> 2)); // 37-42
                data.Add((country >> 2) & 63); // 43-48

                data.Add(((service & 15) << 2) | (country >> 8)); // 49-54
                data.Add(service >> 4); // 60-55

                content = mode2Match.Groups["preamble"].Value + content.Substring(mode2Match.Length);
            }
            else if (mode == 3)
            {
                Match mode3Match = mode3Regex.Match(content);

                if (!mode3Match.Success)
                {
                    mode3Match = mode3GsRegex.Match(content);
                }

                if (!mode3Match.Success)
                {
                    // invalid mode 3 data, convert to mode 4
                    return Analyze(content, 4);
                }

                int service = int.Parse(mode3Match.Groups["service"].Value);
                int country = int.Parse(mode3Match.Groups["country"].Value);
                string zip = mode3Match.Groups["zip"].Value;
                if (zip.Length < 6)
                {
                    zip = zip.PadRight(6);
                }

                long zipval = 0;
                for (int i = 0; i < 6; i++)
                {
                    zipval = zipval << 6 | (long)codeAMap[zip.Substring(i, 1)];
                }

                // ISO/IEC 12023:2000 pp. 26
                data.Add((int)((zipval & 3) << 4) | mode); // 1-6

                data.Add((int)((zipval >> 2) & 63));  // 7-12
                data.Add((int)((zipval >> 8) & 63)); //  13-18
                data.Add((int)((zipval >> 14) & 63)); // 19-24
                data.Add((int)((zipval >> 20) & 63)); // 25-30
                data.Add((int)((zipval >> 26) & 63)); // 31-36

                data.Add(((country & 3) << 4) | (int)((zipval >> 32) & 15)); // 37-42
                data.Add((country >> 2) & 63); // 43-48

                data.Add(((service & 15) << 2) | (country >> 8)); // 49-54
                data.Add(service >> 4); // 60-55

                content = mode3Match.Groups["preamble"].Value + content.Substring(mode3Match.Length);
            }
            else
            {
                data.Add(mode);
                eec = mode == 5;
            }

            while (content.Length > 0)
            {
                if (data.Count == 10)
                {
                    data.AddRange(ComputeReadSolomon(data, ec10Poly));
                }

                if (numericShiftRegex.IsMatch(content))
                {
                    data.Add(codeMap[NS]);
                    int value = int.Parse(content.Substring(0, 9));
                    content = content.Substring(9);
                    for (int i = 24; i >= 0; i -= 6)
                    {
                        data.Add((value >> i) & 63);
                    }
                }
                else
                {
                    string c = StringInfo.GetNextTextElement(content);
                    StringInfo stringInfo = new StringInfo(content);
                    if (stringInfo.LengthInTextElements > 1)
                    {
                        content = stringInfo.SubstringByTextElements(1);
                    }
                    else
                    {
                        content = string.Empty;
                    }

                    if (codeMap.ContainsKey(c))
                    {
                        data.Add(codeMap[c]);
                    }
                    else if (codeSet == MaxiCodeCodeSet.CodeSetA && codeBMap.ContainsKey(c))
                    {
                        if (codeBRegex.IsMatch(content) && !codeARegex.IsMatch(content))
                        {
                            data.Add(codeMap[LATCH_B]);
                            codeSet = MaxiCodeCodeSet.CodeSetB;
                            codeMap = codeBMap;
                            data.Add(codeMap[c]);
                        }
                        else
                        {
                            data.Add(codeMap[SHIFT_B]);
                            data.Add(codeBMap[c]);
                        }
                    }
                    else if (codeSet == MaxiCodeCodeSet.CodeSetB && codeAMap.ContainsKey(c))
                    {
                        Match codeAMatch = codeARegex.Match(content);
                        if (codeAMatch.Success)
                        {
                            if (codeAMatch.Length >= 3)
                            {
                                data.Add(codeMap[LATCH_A]);
                                codeSet = MaxiCodeCodeSet.CodeSetA;
                                codeMap = codeAMap;
                                data.Add(codeMap[c]);
                            }
                            else if (codeAMatch.Length == 2)
                            {
                                data.Add(codeMap[SHIFT_3A]);
                                data.Add(codeAMap[c]);
                                data.Add(codeAMap[content.Substring(0, 1)]);
                                data.Add(codeAMap[content.Substring(1, 1)]);
                                content = content.Substring(2);
                            }
                            else if (codeAMatch.Length == 1)
                            {
                                data.Add(codeMap[SHIFT_2A]);
                                data.Add(codeAMap[c]);
                                data.Add(codeAMap[content.Substring(0, 1)]);
                                content = content.Substring(1);
                            }
                        }
                        else
                        {
                            data.Add(codeMap[SHIFT_A]);
                            data.Add(codeAMap[c]);
                        }
                    }
                    else if (codeCMap.ContainsKey(c))
                    {
                        // TODO: lock in if more than one character
                        data.Add(codeMap[SHIFT_C]);
                        data.Add(codeCMap[c]);
                    }
                    else if (codeDMap.ContainsKey(c))
                    {
                        // TODO: lock in if more than one character
                        data.Add(codeMap[SHIFT_D]);
                        data.Add(codeDMap[c]);
                    }
                    else if (codeEMap.ContainsKey(c))
                    {
                        // TODO: lock in if more than one character
                        data.Add(codeMap[SHIFT_E]);
                        data.Add(codeEMap[c]);
                    }
                    else
                    {
                        // non ISO-8859-1 character, drop
                        // TODO: change ECI
                    }
                }
            }

            if (data.Count <= 10)
            {
                data.AddRange(Enumerable.Repeat(codeMap[PAD], 10 - data.Count));
                data.AddRange(ComputeReadSolomon(data, ec10Poly));
            }

            if (eec)
            {
                if (data.Count <= 88)
                {
                    data.AddRange(Enumerable.Repeat(codeMap[PAD], 88 - data.Count));
                }
                else
                {
                    // too much data for EEC
                    // if mode 5, convert to mode 4, else truncate
                    if (mode == 5)
                    {
                        mode = 4;
                        eec = false;
                        data[0] = mode;
                        data.RemoveRange(10, 10);
                        data.InsertRange(10, ComputeReadSolomon(data.GetRange(0, 10), ec10Poly));
                    }
                    else
                    {
                        data.RemoveRange(88, data.Count - 88);
                    }
                }
            }

            if(!eec)
            {
                if (data.Count <= 104)
                {
                    data.AddRange(Enumerable.Repeat(codeMap[PAD], 104 - data.Count));
                }
                else
                {
                    // too much data for SEC, truncate
                    data.RemoveRange(104, data.Count - 104);
                }

            }

            List<int> odds = new List<int>();
            List<int> evens = new List<int>();
            for (int i = 20; i < data.Count; i += 2)
            {
                odds.Add(data[i]);
                evens.Add(data[i + 1]);
            }

            int[] oddCorrection = [];
            int[] evenCorrection = [];
            if (eec)
            {
                oddCorrection = ComputeReadSolomon(odds, ec28Poly);
                evenCorrection = ComputeReadSolomon(evens, ec28Poly);
            }
            else
            {
                oddCorrection = ComputeReadSolomon(odds, ec20Poly);
                evenCorrection = ComputeReadSolomon(evens, ec20Poly);
            }

            for (int i = 0; i < oddCorrection.Length; i++)
            {
                data.Add(oddCorrection[i]);
                data.Add(evenCorrection[i]);
            }

            return data;
        }

        private static int[] ComputeReadSolomon(List<int> data, int[] ecPoly)
        {
            int ecLen = ecPoly.Length - 1;
            List<int> quotient = GF64PolynomalDivision(data.Concat(Enumerable.Repeat(0, ecLen)).ToArray(), ecPoly);
            return quotient.GetRange(quotient.Count - ecLen, ecLen).ToArray();
        }

        private static List<int> GF64PolynomalDivision(int[] dividend, int[] divisor)
        {
            List<int> result = new List<int>(dividend);
            int normalizer = divisor[0];

            for (int i = 0; i < dividend.Length - (divisor.Length - 1); i++)
            {
                result[i] /= normalizer;

                int coeff = result[i];
                if (coeff != 0)
                {
                    for (int j = 1; j < divisor.Length; j++)
                    {
                        result[i + j] ^= GF64Multiply(divisor[j], coeff);
                    }
                }
            }

            return result;
        }

        private static int GF64Multiply(int a, int b)
        {
            int product = 0;
            while (a != 0 && b != 0)
            {
                if ((b & 1) > 0)
                {
                    product ^= a;
                }

                a <<= 1;
                if ((a & 64) > 0)
                {
                    a ^= 67; // x^6 + x + 1
                }

                b >>= 1;
            }

            return product;
        }

    }
}
