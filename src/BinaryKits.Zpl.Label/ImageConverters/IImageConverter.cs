namespace BinaryKits.Zpl.Label.ImageConverters
{
    public interface IImageConverter
    {
        ImageResult ConvertImage(byte[] imageData);
    }
}
