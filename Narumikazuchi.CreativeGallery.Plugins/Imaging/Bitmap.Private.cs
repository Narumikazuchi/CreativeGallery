namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public partial class Bitmap
{
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
            ComIStreamWrapper streamWrapper = new(stream: stream);
            status = GdiPlus.GdipLoadImageFromStream(stream: streamWrapper,
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

    static private Result<Bitmap> CreateFromHandle(IntPtr handle)
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