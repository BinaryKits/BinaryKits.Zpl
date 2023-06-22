using System;

namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Zebra Download Compression Scheme
    /// </summary>
    public enum ZplCompressionScheme
    {
        /// <summary>
        /// No Compression
        /// </summary>
        None,
        /// <summary>
        /// Alternative Compression Scheme
        /// </summary>
        ACS,
        /// <summary>
        /// Z64 encoding compressed using LZ77 algorithm and encoded in Base64.
        /// </summary>
        Z64,
        /// <summary>
        /// Base64 encoding
        /// </summary>
        B64
    }
}
