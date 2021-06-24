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
