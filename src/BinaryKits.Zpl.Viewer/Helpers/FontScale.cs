using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    // Offset is Intercharacter Gap (in dots), 0 if proportional for details see
    // https://docs.zebra.com/us/en/printers/software/zpl-pg/c-zpl-font-barcodes-fonts-andbar-codes/r-zpl-font-barcodes-proportional-fixed-spacing.html
    using FontScaleDictionary = Dictionary<string, (int height, int width, int offset)>;

    internal static class FontScale
    {
        private static readonly FontScaleDictionary fontScales6mm = new()
        {
            ["A"] = (9, 5, 1),
            ["B"] = (11, 7, 2),
            ["C"] = (18, 10, 2),
            ["D"] = (18, 10, 2),
            ["E"] = (21, 10, 5),
            ["F"] = (26, 13, 3),
            ["G"] = (60, 40, 8),
            ["H"] = (17, 11, 6),
            ["GS"] = (24, 24, 0)
        };

        private static readonly FontScaleDictionary fontScales8mm = new()
        {
            ["A"] = (9, 5, 1),
            ["B"] = (11, 7, 2),
            ["C"] = (18, 10, 2),
            ["D"] = (18, 10, 2),
            ["E"] = (21, 10, 5),
            ["F"] = (26, 13, 3),
            ["G"] = (60, 40, 8),
            ["H"] = (17, 11, 6),
            ["GS"] = (24, 24, 0),
            ["P"] = (20, 18, 0),
            ["Q"] = (28, 24, 0),
            ["R"] = (35, 31, 0),
            ["S"] = (40, 35, 0),
            ["T"] = (48, 42, 0),
            ["U"] = (59, 53, 0),
            ["V"] = (80, 71, 0)
        };

        private static readonly FontScaleDictionary fontScales12mm = new()
        {
            ["A"] = (9, 5, 1),
            ["B"] = (11, 7, 2),
            ["C"] = (18, 10, 2),
            ["D"] = (18, 10, 2),
            ["E"] = (21, 10, 5),
            ["F"] = (26, 13, 3),
            ["G"] = (60, 40, 8),
            ["H"] = (17, 11, 6),
            ["GS"] = (24, 24, 0),
            ["P"] = (20, 18, 0),
            ["Q"] = (28, 24, 0),
            ["R"] = (35, 31, 0),
            ["S"] = (40, 35, 0),
            ["T"] = (48, 42, 0),
            ["U"] = (59, 53, 0),
            ["V"] = (80, 71, 0)
        };

        private static readonly FontScaleDictionary fontScales24mm = new()
        {
            ["A"] = (9, 5, 1),
            ["B"] = (11, 7, 2),
            ["C"] = (18, 10, 2),
            ["D"] = (18, 10, 2),
            ["E"] = (21, 10, 5),
            ["F"] = (26, 13, 3),
            ["G"] = (60, 40, 8),
            ["H"] = (17, 11, 6),
            ["GS"] = (24, 24, 0),
            ["P"] = (20, 18, 0),
            ["Q"] = (28, 24, 0),
            ["R"] = (35, 31, 0),
            ["S"] = (40, 35, 0),
            ["T"] = (48, 42, 0),
            ["U"] = (59, 53, 0),
            ["V"] = (80, 71, 0)
        };

        private static (int height, int width, int offset)? GetFontScale(string fontName, int printDensityDpmm)
        {
            FontScaleDictionary dict;
            switch (printDensityDpmm)
            {
                case 6:
                    dict = fontScales6mm;
                    break;
                case 8:
                    dict = fontScales8mm;
                    break;
                case 12:
                    dict = fontScales12mm;
                    break;
                case 24:
                    dict = fontScales24mm;
                    break;
                default:
                    return null;
            }

            if (dict.TryGetValue(fontName, out (int height, int width, int offset) value))
            {
                return value;
            }

            return null;
        }

        private static (int height, int width) defaultScalingFontScale = (15, 12);

        public static (float fontSize, float scaleX) GetFontScaling(string fontName, int fontHeight, int fontWidth, int printDensityDpmm)
        {
            (int height, int width, int offset)? fontScale = GetFontScale(fontName, printDensityDpmm);

            if (fontScale != null)
            {
                (int height, int width, int offset) = fontScale.Value;
                if (fontHeight > 0)
                {
                    double heightRatio = (double)fontHeight / height;
                    int intHeightRatio = (int)Math.Max(1, Math.Round(heightRatio));
                    float emSize = (height + offset) * intHeightRatio;

                    if (fontWidth == 0)
                    {
                        return (emSize, 1.0f);
                    }

                    double widthRatio = (double)fontWidth / width;
                    int intWidthRatio = (int)Math.Max(1, Math.Round(widthRatio));

                    return (emSize, (float)intWidthRatio / intHeightRatio);
                }
                else if (fontWidth > 0)
                {
                    double widthRatio = (double)fontWidth / width;
                    int intWidthRatio = (int)Math.Max(1, Math.Round(widthRatio));

                    return ((height + offset) * intWidthRatio, 1.0f);
                }
                else
                {
                    return (height + offset, 1.0f);
                }
            }

            float fontSize = fontHeight > 0 ? fontHeight : fontWidth > 0 ? fontWidth : defaultScalingFontScale.height;
            float scaleX = 1.0f;

            if (fontWidth != 0 && fontWidth != fontSize)
            {
                scaleX = fontWidth / fontSize;
            }

            return (fontSize, scaleX);
        }
    }
}
