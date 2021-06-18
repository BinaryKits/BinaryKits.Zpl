using System.Collections.Generic;

namespace BinaryKits.Utility.ZPLUtility.Elements
{
    public abstract class ZPLElementBase
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

        public ZPLElementBase()
        {
            Comments = new List<string>();
            IsEnabled = true;
        }

        public IEnumerable<string> Render()
        {
            return Render(new ZPLRenderOptions());
        }

        public string RenderToString()
        {
            return string.Join(" ", Render());
        }

        public abstract IEnumerable<string> Render(ZPLRenderOptions context);

        public string ToZPLString()
        {
            return ToZPLString(new ZPLRenderOptions());
        }

        public string ToZPLString(ZPLRenderOptions context)
        {
            return string.Join("\n", Render(context));
        }
    }
}
