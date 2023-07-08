﻿using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    /// <summary>
    /// Public interface for command analyzers
    /// </summary>
    public interface IZplCommandAnalyzer
    {
        /// <summary>
        /// Checks if the analyzer can analyze this command
        /// </summary>
        /// <param name="zplLine"></param>
        /// <returns></returns>
        bool CanAnalyze(string zplLine);

        /// <summary>
        /// Analyzes the command a returns the corresponding ZPL Element.
        /// </summary>
        /// <param name="zplCommand">The command to analyze.</param>
        /// <returns></returns>
        ZplElementBase Analyze(string zplCommand);
    }
}
