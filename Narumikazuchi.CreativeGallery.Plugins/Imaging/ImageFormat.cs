namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public sealed class ImageFormat
{
    static public ImageFormat Bmp
    {
        get
        {
            return s_BmpImageFormat.Value;
        }
    }

    static public ImageFormat Emf
    {
        get
        {
            return s_EmfImageFormat.Value;
        }
    }

    static public ImageFormat Exif
    {
        get
        {
            return s_ExifImageFormat.Value;
        }
    }

    static public ImageFormat Gif
    {
        get
        {
            return s_GifImageFormat.Value;
        }
    }

    static public ImageFormat Icon
    {
        get
        {
            return s_IconImageFormat.Value;
        }
    }

    static public ImageFormat Jpeg
    {
        get
        {
            return s_JpegImageFormat.Value;
        }
    }

    static public ImageFormat MemoryBmp
    {
        get
        {
            return s_MemoryBmpImageFormat.Value;
        }
    }

    static public ImageFormat Png
    {
        get
        {
            return s_PngImageFormat.Value;
        }
    }

    static public ImageFormat Tiff
    {
        get
        {
            return s_TiffImageFormat.Value;
        }
    }

    static public ImageFormat Wmf
    {
        get
        {
            return s_WmfImageFormat.Value;
        }
    }

    public override Boolean Equals(Object? obj)
    {
        if (obj is ImageFormat other)
        {
            return other.Guid.Equals(g: this.Guid) is true;
        }
        else
        {
            return false;
        }
    }

    public override Int32 GetHashCode()
    {
        return m_Guid.GetHashCode();
    }

    public Guid Guid
    {
        get
        {
            return m_Guid;
        }
    }

    private ImageFormat(String guid)
    {
        m_Guid = Guid.Parse(input: guid);
    }

    private readonly Guid m_Guid;

    private static readonly Lazy<ImageFormat> s_BmpImageFormat = new(valueFactory: () => new ImageFormat(guid: BMPGUID),
                                                                     mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_EmfImageFormat = new(valueFactory: () => new ImageFormat(guid: EMFGUID),
                                                                     mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_ExifImageFormat = new(valueFactory: () => new ImageFormat(guid: EXIFGUID),
                                                                      mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_GifImageFormat = new(valueFactory: () => new ImageFormat(guid: GIFGUID),
                                                                     mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_TiffImageFormat = new(valueFactory: () => new ImageFormat(guid: TIFFGUID),
                                                                      mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_PngImageFormat = new(valueFactory: () => new ImageFormat(guid: PNGGUID),
                                                                     mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_MemoryBmpImageFormat = new(valueFactory: () => new ImageFormat(guid: MEMORYBMPGUID),
                                                                           mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_IconImageFormat = new(valueFactory: () => new ImageFormat(guid: ICONGUID),
                                                                      mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_JpegImageFormat = new(valueFactory: () => new ImageFormat(guid: JPEGGUID),
                                                                      mode: LazyThreadSafetyMode.ExecutionAndPublication);
    private static readonly Lazy<ImageFormat> s_WmfImageFormat = new(valueFactory: () => new ImageFormat(guid: WMFGUID),
                                                                     mode: LazyThreadSafetyMode.ExecutionAndPublication);

    private const String BMPGUID = "b96b3cab-0728-11d3-9d7b-0000f81ef32e";
    private const String EMFGUID = "b96b3cac-0728-11d3-9d7b-0000f81ef32e";
    private const String EXIFGUID = "b96b3cb2-0728-11d3-9d7b-0000f81ef32e";
    private const String GIFGUID = "b96b3cb0-0728-11d3-9d7b-0000f81ef32e";
    private const String TIFFGUID = "b96b3cb1-0728-11d3-9d7b-0000f81ef32e";
    private const String PNGGUID = "b96b3caf-0728-11d3-9d7b-0000f81ef32e";
    private const String MEMORYBMPGUID = "b96b3caa-0728-11d3-9d7b-0000f81ef32e";
    private const String ICONGUID = "b96b3cb5-0728-11d3-9d7b-0000f81ef32e";
    private const String JPEGGUID = "b96b3cae-0728-11d3-9d7b-0000f81ef32e";
    private const String WMFGUID = "b96b3cad-0728-11d3-9d7b-0000f81ef32e";
}