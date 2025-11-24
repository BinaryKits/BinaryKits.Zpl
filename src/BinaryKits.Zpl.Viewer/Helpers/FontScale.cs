using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    // Offset is Intercharacter Gap (in dots), 0 if proportional
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
            float fontSize, scaleX;

            (int height, int width, int offset)? fontScale = GetFontScale(fontName, printDensityDpmm);

            if (fontScale != null)
            {
                (int height, int width, int offset) = fontScale.Value;
                if (fontHeight > 0)
                {
                    double heightRatio = (double)fontHeight / height;
                    int intHeightRatio = (int)Math.Round(heightRatio);
                    fontSize = (height + offset) * Math.Max(1, intHeightRatio);

                    if (fontWidth == 0)
                    {
                        return (fontSize, 1.0f);
                    }

                    double widthRatio = (double)fontWidth / width;
                    int intWidthRatio = (int)Math.Round(widthRatio);

                    scaleX = (float)intWidthRatio / intHeightRatio;
                    return (fontSize, scaleX);
                }
                else if (fontWidth > 0)
                {
                    double widthRatio = (double)fontWidth / width;
                    int intWidthRatio = (int)Math.Round(widthRatio);
                    fontSize = (height + offset) * Math.Max(1, intWidthRatio);

                    return (fontSize, 1.0f);

                }
            }

            fontSize = fontHeight > 0 ? fontHeight : fontWidth > 0 ? fontWidth : defaultScalingFontScale.height;
            scaleX = 1.0f;

            if (fontWidth != 0 && fontWidth != fontSize)
            {
                scaleX = fontWidth / fontSize;
            }

            return (fontSize, scaleX);
        }
    }
}
