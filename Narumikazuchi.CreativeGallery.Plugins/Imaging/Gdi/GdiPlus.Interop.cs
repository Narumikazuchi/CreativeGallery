#pragma warning disable SYSLIB1054

namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

static internal partial class GdiPlus
{
    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipGetImageType(IntPtr handle,
                                                          out ImageType type);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipDrawImageRectRectI(IntPtr graphics,
                                                                IntPtr handle,
                                                                Int32 dstx,
                                                                Int32 dsty,
                                                                Int32 dstwidth,
                                                                Int32 dstheight,
                                                                Int32 srcx,
                                                                Int32 srcy,
                                                                Int32 srcwidth,
                                                                Int32 srcheight,
                                                                GraphicsUnit srcUnit,
                                                                IntPtr imageattr,
                                                                IntPtr callback,
                                                                IntPtr callbackData);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipLoadImageFromDelegate_linux(StreamGetHeaderDelegate getHeader,
                                                                         StreamGetBytesDelegate getBytes,
                                                                         StreamPutBytesDelegate putBytes,
                                                                         StreamSeekDelegate doSeek,
                                                                         StreamCloseDelegate close,
                                                                         StreamSizeDelegate size,
                                                                         out IntPtr handle);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipSaveImageToDelegate_linux(IntPtr handle,
                                                                       StreamGetBytesDelegate getBytes,
                                                                       StreamPutBytesDelegate putBytes,
                                                                       StreamSeekDelegate doSeek,
                                                                       StreamCloseDelegate close,
                                                                       StreamSizeDelegate size,
                                                                       ref Guid encoderClsID,
                                                                       IntPtr encoderParameters);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipLoadImageFromStream([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ComIStreamMarshaler))] IStream stream,
                                                                 out IntPtr handle);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipSaveImageToStream(HandleRef handle,
                                                               [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ComIStreamMarshaler))] IStream stream,
                                                               [In()] ref Guid clsidEncoder,
                                                               HandleRef encoderParams);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipCreateBitmapFromScan0(Int32 width,
                                                                   Int32 height,
                                                                   Int32 stride,
                                                                   PixelFormat format,
                                                                   IntPtr scan0,
                                                                   out IntPtr bmp);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipGetImageWidth(IntPtr handle,
                                                           out UInt32 width);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipGetImageHeight(IntPtr handle,
                                                            out UInt32 height);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdiplusStartup(ref UInt64 token,
                                                        ref GdiplusStartupInput input,
                                                        ref GdiplusStartupOutput output);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipDeleteGraphics(IntPtr handle);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipGetImageGraphicsContext(IntPtr handle,
                                                                     out IntPtr graphics);

    [DllImport(GDIPLUSDLL)]
    extern static internal GdiPlusStatus GdipSetVisibleClip_linux(IntPtr handle,
                                                                  ref Rectangle rectangle);

    [DllImport(GDIPLUSDLL)]
    extern static internal Int32 GdipGetImageEncodersSize(out Int32 encoderNums,
                                                          out Int32 arraySize);

    [DllImport(GDIPLUSDLL)]
    extern static internal Int32 GdipGetImageEncoders(Int32 encoderNums,
                                                      Int32 arraySize,
                                                      IntPtr encoders);

    private const String GDIPLUSDLL = "gdiplus";
}