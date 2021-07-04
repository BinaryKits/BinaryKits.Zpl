using System;
using System.Collections.Generic;

namespace BinaryKits.ZplUtility.Elements
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

        public IEnumerable<string> Render()
        {
            return Render(new ZplRenderOptions());
        }

        public string RenderToString()
        {
            return string.Join(" ", Render());
        }

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
