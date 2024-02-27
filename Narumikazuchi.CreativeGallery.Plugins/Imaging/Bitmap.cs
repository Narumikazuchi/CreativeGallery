namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

/// <summary>
/// Can represent any file of one of the following formats: bmp, jpg, png, gif, tif
/// </summary>
public sealed partial class Bitmap
{
    static public Bitmap CreateEmpty(Int32 width,
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

    static public Result<Bitmap> LoadFromStream<TStream>(TStream stream)
        where TStream : Stream
    {
        IntPtr handle = InitializeFromStream(stream: stream);
        return CreateFromHandle(handle: handle);
    }

    public Result<Bitmap> GetThumbnailImage(Int32 width,
                                            Int32 height)
    {
        if (width <= 0 ||
            height <= 0)
        {
            return new OutOfMemoryException(message: DIMENSIONS_CANNOT_BE_NEGATIVE);
        }

        Bitmap result = CreateEmpty(width: width,
                                    height: height);

        using Graphics graphics = Graphics.FromImage(image: result);
        GdiPlusStatus status = GdiPlus.GdipDrawImageRectRectI(graphics: graphics.Handle,
                                                              handle: this.Handle,
                                                              dstx: 0,
                                                              dsty: 0,
                                                              dstwidth: width,
                                                              dstheight: height,
                                                              srcx: 0,
                                                              srcy: 0,
                                                              srcwidth: this.Width,
                                                              srcheight: this.Height,
                                                              srcUnit: GraphicsUnit.Pixel,
                                                              imageattr: IntPtr.Zero,
                                                              callback: IntPtr.Zero,
                                                              callbackData: IntPtr.Zero);

        GdiPlus.ThrowIfFailed(status: status);

        return result;
    }

    public Optional<Exception> SaveInto<TStream>(TStream stream,
                                                 ImageFormat format)
        where TStream : Stream
    {
        Optional<ImageCodecInfo> encoder = CreateEncoderForFormat(format: format);
        if (encoder.HasValue is false)
        {
            return new KeyNotFoundException(message: CODEC_FOR_FORMAT_NOT_FOUND);
        }

        this.SaveInto(stream: stream,
                      encoder: encoder.Value);
        return default;
    }

    public void SaveInto<TStream>(TStream stream,
                                  ImageCodecInfo encoder)
        where TStream : Stream
    {
        GdiPlusStatus status;
        Guid guid = encoder.Clsid;

        if (GdiPlus.UseLinuxDraw is true)
        {
            GdiPlusStreamHelper helper = new(stream: stream,
                                             seekToOrigin: false);
            status = GdiPlus.GdipSaveImageToDelegate_linux(handle: this.Handle,
                                                           getBytes: helper.GetBytesDelegate,
                                                           putBytes: helper.PutBytesDelegate,
                                                           doSeek: helper.SeekDelegate,
                                                           close: helper.CloseDelegate,
                                                           size: helper.SizeDelegate,
                                                           encoderClsID: ref guid,
                                                           encoderParameters: IntPtr.Zero);
        }
        else
        {
            HandleRef handleRef = new(wrapper: this,
                                      handle: this.Handle);
            ComIStreamWrapper streamWrapper = new(stream: stream);
            status = GdiPlus.GdipSaveImageToStream(handle: handleRef,
                                                   stream: streamWrapper,
                                                   clsidEncoder: ref guid,
                                                   encoderParams: default);
        }

        GdiPlus.ThrowIfFailed(status: status);
    }

    public Int32 Width
    {
        get
        {
            GdiPlusStatus status = GdiPlus.GdipGetImageWidth(handle: this.Handle,
                                                             width: out UInt32 width);
            GdiPlus.ThrowIfFailed(status: status);
            return (Int32)width;
        }
    }

    public Int32 Height
    {
        get
        {
            GdiPlusStatus status = GdiPlus.GdipGetImageHeight(handle: this.Handle,
                                                              height: out UInt32 height);
            GdiPlus.ThrowIfFailed(status: status);
            return (Int32)height;
        }
    }

    internal Bitmap(IntPtr handle)
    {
        this.Handle = handle;
    }

    internal IntPtr Handle
    {
        get;
    }
}