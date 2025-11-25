using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Viewer.Helpers
{
    using FontScaleDictionary = Dictionary<string, (int height, int width)>;

    internal static class FontScale
    {
        private static readonly FontScaleDictionary fontScales6mm = new()
        {
            ["A"] = (9, 5),
            ["B"] = (11, 7),
            ["C"] = (18, 10),
            ["D"] = (18, 10),
            ["E"] = (21, 10),
            ["F"] = (26, 13),
            ["G"] = (60, 40),
            ["H"] = (17, 11),
            ["GS"] = (24, 24)
        };

        private static readonly FontScaleDictionary fontScales8mm = new()
        {
            ["A"] = (9, 5),
            ["B"] = (11, 7),
            ["C"] = (18, 10),
            ["D"] = (18, 10),
            ["E"] = (28, 15),
            ["F"] = (26, 13),
            ["G"] = (60, 40),
            ["H"] = (21, 13),
            ["GS"] = (24, 24),
            ["P"] = (20, 18),
            ["Q"] = (28, 24),
            ["R"] = (35, 31),
            ["S"] = (40, 35),
            ["T"] = (48, 42),
            ["U"] = (59, 53),
            ["V"] = (80, 71)
        };

        private static readonly FontScaleDictionary fontScales12mm = new()
        {
            ["A"] = (9, 5),
            ["B"] = (11, 7),
            ["C"] = (18, 10),
            ["D"] = (18, 10),
            ["E"] = (42, 20),
            ["F"] = (26, 13),
            ["G"] = (60, 40),
            ["H"] = (34, 22),
            ["GS"] = (24, 24),
            ["P"] = (20, 18),
            ["Q"] = (28, 24),
            ["R"] = (35, 31),
            ["S"] = (40, 35),
            ["T"] = (48, 42),
            ["U"] = (59, 53),
            ["V"] = (80, 71)
        };

        private static readonly FontScaleDictionary fontScales24mm = new()
        {
            ["A"] = (9, 5),
            ["B"] = (11, 7),
            ["C"] = (18, 10),
            ["D"] = (18, 10),
            ["E"] = (42, 20),
            ["F"] = (26, 13),
            ["G"] = (60, 40),
            ["H"] = (34, 22),
            ["GS"] = (24, 24),
            ["P"] = (20, 18),
            ["Q"] = (28, 24),
            ["R"] = (35, 31),
            ["S"] = (40, 35),
            ["T"] = (48, 42),
            ["U"] = (59, 53),
            ["V"] = (80, 71)
        };

        private static (int height, int width)? GetFontScale(string fontName, int printDensityDpmm)
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

            if (dict.TryGetValue(fontName, out (int height, int width) value))
            {
                return value;
            }

            return null;
        }

        private static readonly (int height, int width) defaultScalingFontScale = (15, 12);

        // This is a corrective used to match labelary.com scale for text fields' font size
        private const float heightScale = 1.1f;

        public static float? GetBitmappedFontSize(string fontName, int scalingFactor, int printDensityDpmm)
        {
            return GetFontScale(fontName, printDensityDpmm)?.height * scalingFactor * heightScale;
        }

        public static (float fontSize, float scaleX) GetFontScaling(string fontName, int fontHeight, int fontWidth, int printDensityDpmm)
        {
            (int height, int width)? fontScale = GetFontScale(fontName, printDensityDpmm);

            if (fontScale != null)
            {
                (int height, int width) = fontScale.Value;
                if (fontHeight > 0)
                {
                    double heightRatio = (double)fontHeight / height;
                    int intHeightRatio = (int)Math.Max(1, Math.Round(heightRatio));
                    float emSize = height * intHeightRatio * heightScale;

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

                    return (height * intWidthRatio * heightScale, 1.0f);
                }
                else
                {
                    return (height * heightScale, 1.0f);
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
