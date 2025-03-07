namespace MauiCameraView.Utilities;

public interface IImageDevice
{
    public int MaxWidthHeight { get; set; }

    public int CompressionQuality { get; set; }

    public double CustomPhotoSize { get; set; }

    Task<byte[]?> ProcesarPhoto(FileResult simagen);

    Task<byte[]?> ProcesarPhoto(Stream sphoto);
}
