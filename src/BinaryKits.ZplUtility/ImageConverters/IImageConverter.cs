namespace BinaryKits.ZplUtility.ImageConverters
{
    public interface IImageConverter
    {
        ImageResult ConvertImage(byte[] imageData);
    }
}
