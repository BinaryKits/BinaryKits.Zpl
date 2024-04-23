using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using SkiaSharp;
using System;
using System.Collections.Generic;
using ZXing;
using ZXing.Common;
using ZXing.PDF417;
using ZXing.PDF417.Internal;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    /// <summary>
    /// Drawer for PDF417 Barcode elements
    /// </summary>
    public class Pdf417ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplPDF417;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            if (element is ZplPDF417 pdf417)
            {
                if (pdf417.Height == 0)
                    throw new System.Exception("PDF417 Height is set to zero.");

                if (string.IsNullOrWhiteSpace(pdf417.Content))
                    throw new System.Exception("PDF147 Content is empty.");

                float x = pdf417.PositionX;
                float y = pdf417.PositionY;

                int mincols, maxcols, minrows, maxrows;
                if (pdf417.Rows != null)
                {
                    minrows = pdf417.Rows.Value;
                    maxrows = pdf417.Rows.Value;
                }
                else
                {
                    minrows = 3;
                    maxrows = 90;
                }

                if (pdf417.Columns != null)
                {
                    mincols = pdf417.Columns.Value;
                    maxcols = pdf417.Columns.Value;
                }
                else
                {
                    mincols = 1;
                    maxcols = 30;

                    if (pdf417.Rows != null)
                    {
                        //When column count isn't defined, and rows count is,
                        // the col/row ratio is calculated using the algorithm in: ZXing.PDF417.Internal.PDF417.determineDimensions
                        // to allow a range for that algorithm, we divide the given row amount by 2.
                        // as the algorithm goes from highest to lowest, this usually produces an acceptable result 
                        minrows /= 2;
                    }
                }
                var writer = new PDF417Writer();
                var hints = new Dictionary<EncodeHintType, object> {
                    // { EncodeHintType.CHARACTER_SET, "ISO-8859-1" },
                    { EncodeHintType.PDF417_COMPACT, pdf417.Compact },
                    //{ EncodeHintType.PDF417_AUTO_ECI, true },
                    //{ EncodeHintType.DISABLE_ECI, true },
                    { EncodeHintType.PDF417_COMPACTION, Compaction.AUTO},
                    { EncodeHintType.PDF417_ASPECT_RATIO, PDF417AspectRatio.A3 }, // height of a single bar relative to width
                    { EncodeHintType.PDF417_IMAGE_ASPECT_RATIO, 1.0f }, // zpl default 2.0f, proportions of columns to rows //1.0f looks closer to printed Zebra label
                    { EncodeHintType.MARGIN, 0 }, // its an int
                    { EncodeHintType.ERROR_CORRECTION, ConvertErrorCorrection(pdf417.SecurityLevel) },
                    { EncodeHintType.PDF417_DIMENSIONS, new Dimensions(mincols, maxcols, minrows, maxrows) },
                };
            
                var default_bitmatrix = writer.encode(pdf417.Content, BarcodeFormat.PDF_417, 0, 0, hints);
                
                //PDF417_ASPECT_RATIO set to 3, we need to multiply that with pdf417.ModuleWidth (defined by ^BY)
                var bar_height = pdf417.ModuleWidth * 3;
                var upscaled = proportional_upscale(default_bitmatrix, pdf417.ModuleWidth);
                var result = vertical_scale(upscaled, pdf417.Height, bar_height);
                
                using var resizedImage = this.BitMatrixToSKBitmap(result, 1);
                {
                    var png = resizedImage.Encode(SKEncodedImageFormat.Png, 100).ToArray();
                    this.DrawBarcode(png, resizedImage.Height, resizedImage.Width, pdf417.FieldOrigin != null, x, y, 0, pdf417.FieldOrientation);
                }
            }
        }

        // bitmatrix scaling instead of bitmap
        private BitMatrix proportional_upscale(BitMatrix old, int scale) {
            if (scale == 0 || scale == 1)
            {
                return old;
            }
            BitMatrix upscaled = new BitMatrix(old.Width * scale, old.Height * scale);
            for (int i = 0; i < old.Height; i++)
            {
                BitArray old_row = old.getRow(i, null);
                for (int j = 0; j < old.Width; j++)
                {
                    bool is_set = old_row[j];
                    if (!is_set)
                    {
                        continue;
                    }
                    upscaled.setRegion(j * scale, i * scale, scale, scale);
                }
            }
            return upscaled;
        }

        // needed to match zebra and labelary
        // zebra assumptions:
        //  - we can only set the height in zpl in points, not the width
        //  - each bar is ^BY "points" thick
        //  - because we have PDF417_ASPECT_RATIO set to 3, the height of a single bar is now 3 * ^BY
        private BitMatrix vertical_scale(BitMatrix old_matrix, int new_bar_height, int old_bar_height) {
            int width = old_matrix.Width;
            int rows = old_matrix.Height / old_bar_height; // logical rows;

            if (new_bar_height == old_bar_height || new_bar_height == 0)
            {
                return old_matrix;
            }

            BitMatrix scaled = new BitMatrix(old_matrix.Width, rows * new_bar_height);

            for (int i = 0; i < rows; i++)
            {
                BitArray old_row = old_matrix.getRow(i * old_bar_height, null);
                for (int j = 0; j < width; j++)
                {
                    bool is_set = old_row[j];
                    if (!is_set)
                    {
                        continue;
                    }
                    scaled.setRegion(j, i * new_bar_height, 1, new_bar_height);
                }
            }
            return scaled;
        }

        private PDF417ErrorCorrectionLevel ConvertErrorCorrection(int correction)
        {
            return correction switch
            {
                0 => PDF417ErrorCorrectionLevel.L0,
                1 => PDF417ErrorCorrectionLevel.L1,
                2 => PDF417ErrorCorrectionLevel.L2,
                3 => PDF417ErrorCorrectionLevel.L3,
                4 => PDF417ErrorCorrectionLevel.L4,
                5 => PDF417ErrorCorrectionLevel.L5,
                6 => PDF417ErrorCorrectionLevel.L6,
                7 => PDF417ErrorCorrectionLevel.L7,
                8 => PDF417ErrorCorrectionLevel.L8,
                _ => PDF417ErrorCorrectionLevel.AUTO,
            };
        }
    }
}
