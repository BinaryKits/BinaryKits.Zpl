namespace BinaryKits.Zpl.Label.Elements
{
    /// <summary>
    /// Implemented by elements allowing their content to be set
    /// when merging formats (templating).
    /// </summary>
    public interface IFormatElement
    {
        /// <summary>
        /// Sets element's content (text, bar code value...).
        /// </summary>
        /// <param name="content">String content</param>
        void SetTemplateContent(string content);
    }
}