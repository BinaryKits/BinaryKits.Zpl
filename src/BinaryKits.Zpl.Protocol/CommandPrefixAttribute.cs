using System;

namespace BinaryKits.Zpl.Protocol
{
    public class CommandPrefixAttribute : Attribute
    {
        public string Prefix { get; set; }

        public CommandPrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }

    }
}
