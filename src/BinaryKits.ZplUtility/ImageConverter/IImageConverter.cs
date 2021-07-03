namespace BinaryKits.ZplUtility.ImageConverter
{
    public interface IImageConverter
    {
        ImageResult ConvertImage(byte[] imageData);
    }
}
