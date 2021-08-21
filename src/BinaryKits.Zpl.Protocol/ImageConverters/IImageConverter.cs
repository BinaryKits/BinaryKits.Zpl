namespace BinaryKits.Zpl.Protocol.ImageConverters
{
    /// <summary>
    /// Image Converter
    /// </summary>
    public interface IImageConverter
    {
        /// <summary>
        /// Convert image to bitonal image (grf)
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        ImageResult ConvertImage(byte[] imageData);

        /// <summary>
        /// Convert from bitonal image (grf) to png image
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="bytesPerRow"></param>
        /// <returns></returns>
        byte[] ConvertImage(byte[] imageData, int bytesPerRow);
    }
}
