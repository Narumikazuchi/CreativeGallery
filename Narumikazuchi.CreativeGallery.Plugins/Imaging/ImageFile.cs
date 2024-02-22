namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public abstract partial class ImageFile
{
    static public Result<ImageFile> LoadFromStream<TStream>(TStream stream)
        where TStream : Stream
    {
        IntPtr handle = InitializeFromStream(stream: stream);
        return CreateFromHandle(handle: handle);
    }

    public Result<ImageFile> GetThumbnailImage(Int32 width,
                                               Int32 height)
    {
        if (width <= 0 ||
            height <= 0)
        {
            return new OutOfMemoryException(message: DIMENSIONS_CANNOT_BE_NEGATIVE);
        }

        ImageFile result = Bitmap.CreateEmpty(width: width,
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
            status = GdiPlus.GdipSaveImageToStream(handle: new(wrapper: this,
                                                               handle: this.Handle),
                                                   stream: new ComIStreamWrapper(stream: stream),
                                                   clsidEncoder: ref guid,
                                                   encoderParams: new HandleRef());
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

    internal IntPtr Handle
    {
        get;
    }

    protected ImageFile(IntPtr handle)
    {
        this.Handle = handle;
    }

    static private IntPtr InitializeFromStream<TStream>(TStream stream)
        where TStream : Stream
    {
        if (stream.CanSeek is false)
        {
            Byte[] buffer = new Byte[256];
            Int32 index = 0;
            Int32 count;

            do
            {
                if (buffer.Length < index + 256)
                {
                    Byte[] biggerBuffer = new Byte[buffer.Length * 2];
                    buffer.AsSpan()
                          .CopyTo(destination: biggerBuffer);
                    buffer = biggerBuffer;
                }

                count = stream.Read(buffer: buffer,
                                    offset: index,
                                    count: 256);
                index += count;
            }
            while (count is not 0);

            using MemoryStream memoryStream = new(buffer: buffer);
            IntPtr result = InitializeFromStream(stream: memoryStream);
            return result;
        }

        GdiPlusStatus status;
        IntPtr handle;
        if (GdiPlus.UseLinuxDraw is true)
        {
            GdiPlusStreamHelper helper = new(stream: stream,
                                             seekToOrigin: true);
            status = GdiPlus.GdipLoadImageFromDelegate_linux(getHeader: helper.GetHeaderDelegate,
                                                             getBytes: helper.GetBytesDelegate,
                                                             putBytes: helper.PutBytesDelegate,
                                                             doSeek: helper.SeekDelegate,
                                                             close: helper.CloseDelegate,
                                                             size: helper.SizeDelegate,
                                                             handle: out handle);
        }
        else
        {
            status = GdiPlus.GdipLoadImageFromStream(stream: new ComIStreamWrapper(stream: stream),
                                                     handle: out handle);
        }

        if (status is GdiPlusStatus.Ok)
        {
            return handle;
        }
        else
        {
            return IntPtr.Zero;
        }
    }

    static private Result<ImageFile> CreateFromHandle(IntPtr handle)
    {
        GdiPlusStatus status = GdiPlus.GdipGetImageType(handle: handle,
                                                        type: out ImageType type);
        GdiPlus.ThrowIfFailed(status: status);

        return type switch
        {
            ImageType.Bitmap => new Bitmap(handle: handle),
            _ => new NotSupportedException(message: IMAGE_TYPE_NOT_SUPPORTED),
        };
    }

    static private Optional<ImageCodecInfo> CreateEncoderForFormat(ImageFormat format)
    {
        ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

        if (format.Guid.Equals(g: ImageFormat.MemoryBmp.Guid) is true)
        {
            format = ImageFormat.Png;
        }

        for (Int32 index = 0;
             index < encoders.Length;
             index++)
        {
            if (encoders[index].FormatID.Equals(g: format.Guid) is true)
            {
                return encoders[index];
            }
        }

        return default;
    }

    private const String IMAGE_TYPE_NOT_SUPPORTED = "The image type you tried to load is not supported.";
    private const String DIMENSIONS_CANNOT_BE_NEGATIVE = "The dimensions for the thumbnail must be greater than 0.";
    private const String CODEC_FOR_FORMAT_NOT_FOUND = "The was no codec found for the specified image type.";
}