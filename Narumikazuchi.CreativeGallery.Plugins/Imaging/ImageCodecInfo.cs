namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public readonly struct ImageCodecInfo
{
    static public ImageCodecInfo[] GetImageEncoders()
    {
        return s_ImageCodecs.Value;
    }

    public readonly Guid Clsid
    {
        get;
        init;
    }

    public readonly Guid FormatID
    {
        get;
        init;
    }

    public readonly IntPtr CodecName
    {
        get;
        init;
    }

    public readonly IntPtr DllName
    {
        get;
        init;
    }

    public readonly IntPtr FormatDescription
    {
        get;
        init;
    }

    public readonly IntPtr FilenameExtension
    {
        get;
        init;
    }

    public readonly IntPtr MimeType
    {
        get;
        init;
    }

    public readonly Int32 Flags
    {
        get;
        init;
    }

    public readonly Int32 Version
    {
        get;
        init;
    }

    public readonly Int32 SigCount
    {
        get;
        init;
    }

    public readonly Int32 SigSize
    {
        get;
        init;
    }

    public readonly IntPtr SigPattern
    {
        get;
        init;
    }

    public readonly IntPtr SigMask
    {
        get;
        init;
    }

    static private ImageCodecInfo[] LoadCodecs()
    {
        _ = GdiPlus.GdipGetImageEncodersSize(encoderNums: out Int32 count,
                                             arraySize: out Int32 byteSize);
        ImageCodecInfo[] result = new ImageCodecInfo[count];
        GCHandle handle = GCHandle.Alloc(value: result,
                                         type: GCHandleType.Pinned);

        _ = GdiPlus.GdipGetImageEncoders(encoderNums: count,
                                         arraySize: byteSize,
                                         encoders: handle.AddrOfPinnedObject());

        handle.Free();

        return result;
    }

    static private readonly Lazy<ImageCodecInfo[]> s_ImageCodecs = new(valueFactory: LoadCodecs,
                                                                       mode: LazyThreadSafetyMode.ExecutionAndPublication);
}