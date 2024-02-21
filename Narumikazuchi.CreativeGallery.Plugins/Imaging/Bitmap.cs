namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public sealed class Bitmap : ImageFile
{
    static internal Bitmap CreateEmpty(Int32 width,
                                       Int32 height)
    {
        GdiPlusStatus status = GdiPlus.GdipCreateBitmapFromScan0(width: width,
                                                                 height: height,
                                                                 stride: 0,
                                                                 format: PixelFormat.Format32bppArgb,
                                                                 scan0: IntPtr.Zero,
                                                                 bmp: out IntPtr handle);
        GdiPlus.ThrowIfFailed(status: status);
        return new(handle: handle);
    }

    internal Bitmap(IntPtr handle) :
        base(handle: handle)
    { }
}