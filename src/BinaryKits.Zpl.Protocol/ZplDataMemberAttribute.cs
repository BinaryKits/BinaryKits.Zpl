using System;

namespace BinaryKits.Zpl.Protocol
{
    internal class ZplDataMemberAttribute : Attribute
    {
        public int Order { get; set; } = 0;

        public string PreceedingDelimiter { get; set; } = ",";

        public bool EmitDefault { get; set; } = false;

        public string FormatString { get; set; } = "{0}";

    }
}
