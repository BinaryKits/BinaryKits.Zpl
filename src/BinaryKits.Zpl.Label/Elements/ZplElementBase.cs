using System;
using System.Collections.Generic;

namespace BinaryKits.Zpl.Label.Elements
{
    public abstract class ZplElementBase
    {
        public List<string> Comments { get; protected set; }

        /// <summary>
        /// Indicate the rendering process whether this elemenet can be skipped
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Optionally identify the element for future lookup/manipulation
        /// </summary>
        public string Id { get; set; }

        public ZplElementBase()
        {
            Comments = new List<string>();
            IsEnabled = true;
        }

        /// <summary>
        /// Render Zpl data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Render()
        {
            return Render(new ZplRenderOptions());
        }

        public string RenderToString()
        {
            return string.Join(" ", Render());
        }

        /// <summary>
        /// Render Zpl data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract IEnumerable<string> Render(ZplRenderOptions context);

        public string RenderFieldOrientation(FieldOrientation fieldOrientation)
        {
            switch (fieldOrientation)
            {
                case FieldOrientation.Normal:
                    return "N";
                case FieldOrientation.Rotated90:
                    return "R";
                case FieldOrientation.Rotated180:
                    return "I";
                case FieldOrientation.Rotated270:
                    return "B";
            }

            throw new NotImplementedException("Unknown Field Orientation");
        }

        public string RenderLineColor(LineColor lineColor)
        {
            switch (lineColor)
            {
                case LineColor.Black:
                    return "B";
                case LineColor.White:
                    return "W";
            }

            throw new NotImplementedException("Unknown Line Color");
        }

        public string RenderErrorCorrectionLevel(ErrorCorrectionLevel errorCorrectionLevel)
        {
            switch (errorCorrectionLevel)
            {
                case ErrorCorrectionLevel.UltraHighReliability:
                    return "H";
                case ErrorCorrectionLevel.HighReliability:
                    return "Q";
                case ErrorCorrectionLevel.Standard:
                    return "M";
                case ErrorCorrectionLevel.HighDensity:
                    return "L";
            }

            throw new NotImplementedException("Unknown Error Correction Level");
        }

        public string ToZplString()
        {
            return ToZplString(new ZplRenderOptions());
        }

        public string ToZplString(ZplRenderOptions context)
        {
            return string.Join("\n", Render(context));
        }
    }
}
