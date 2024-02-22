namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

static internal partial class GdiPlus
{
    internal delegate Int32 StreamGetHeaderDelegate(IntPtr buf,
                                                    Int32 bufsz);

    internal delegate Int32 StreamGetBytesDelegate(IntPtr buf,
                                                   Int32 bufsz,
                                                   Boolean peek);

    internal delegate Int64 StreamSeekDelegate(Int32 offset,
                                               Int32 whence);

    internal delegate Int32 StreamPutBytesDelegate(IntPtr buf,
                                                   Int32 bufsz);

    internal delegate void StreamCloseDelegate();

    internal delegate Int64 StreamSizeDelegate();

    static GdiPlus()
    {
        Boolean isLinux = OperatingSystem.IsLinux();
        if (isLinux is true)
        {
            UseLinuxDraw = true;
        }

        GdiplusStartupInput input = GdiplusStartupInput.MakeGdiplusStartupInput();
        GdiplusStartupOutput output = GdiplusStartupOutput.MakeGdiplusStartupOutput();
        try
        {
            GdiplusStartup(token: ref s_GdiPlusToken,
                           input: ref input,
                           output: ref output);
        }
        catch (TypeInitializationException)
        {
            Console.Error.WriteLine(value: GDIPLUS_NOT_INITIALIZED);
        }
    }

    static internal void ThrowIfFailed(GdiPlusStatus status)
    {
        switch (status)
        {
            case GdiPlusStatus.Ok:
                return;
            /*
            case GdiPlusStatus.GenericError:
                msg = Locale.GetText("Generic Error [GDI+ status: {0}]", status);
                throw new Exception(msg);
            case GdiPlusStatus.InvalidParameter:
                msg = Locale.GetText("A null reference or invalid value was found [GDI+ status: {0}]", status);
                throw new ArgumentException(msg);
            case GdiPlusStatus.OutOfMemory:
                msg = Locale.GetText("Not enough memory to complete operation [GDI+ status: {0}]", status);
                throw new OutOfMemoryException(msg);
            case GdiPlusStatus.ObjectBusy:
                msg = Locale.GetText("Object is busy and cannot state allow this operation [GDI+ status: {0}]", status);
                throw new MemberAccessException(msg);
            case GdiPlusStatus.InsufficientBuffer:
                msg = Locale.GetText("Insufficient buffer provided to complete operation [GDI+ status: {0}]", status);
                throw new InternalBufferOverflowException(msg);
            case GdiPlusStatus.PropertyNotSupported:
                msg = Locale.GetText("Property not supported [GDI+ status: {0}]", status);
                throw new NotSupportedException(msg);
            case GdiPlusStatus.FileNotFound:
                msg = Locale.GetText("Requested file was not found [GDI+ status: {0}]", status);
                throw new FileNotFoundException(msg);
            case GdiPlusStatus.AccessDenied:
                msg = Locale.GetText("Access to resource was denied [GDI+ status: {0}]", status);
                throw new UnauthorizedAccessException(msg);
            case GdiPlusStatus.UnknownImageFormat:
                msg = Locale.GetText("Either the image format is unknown or you don't have the required libraries to decode this format [GDI+ status: {0}]", status);
                throw new NotSupportedException(msg);
            case GdiPlusStatus.NotImplemented:
                msg = Locale.GetText("The requested feature is not implemented [GDI+ status: {0}]", status);
                throw new NotImplementedException(msg);
            case GdiPlusStatus.WrongState:
                msg = Locale.GetText("Object is not in a state that can allow this operation [GDI+ status: {0}]", status);
                throw new InvalidOperationException(msg);
            case GdiPlusStatus.FontFamilyNotFound:
                msg = Locale.GetText("The requested FontFamily could not be found [GDI+ status: {0}]", status);
                throw new ArgumentException(msg);
            case GdiPlusStatus.ValueOverflow:
                msg = Locale.GetText("Argument is out of range [GDI+ status: {0}]", status);
                throw new OverflowException(msg);
            case GdiPlusStatus.Win32Error:
                msg = Locale.GetText("The operation is invalid [GDI+ status: {0}]", status);
                throw new InvalidOperationException(msg);
            */
            default:
                throw new Exception(message: $"Unknown Error [GDI+ status: {status}]");
        }
    }

    static internal Boolean UseLinuxDraw
    {
        get;
    }

    static private UInt64 s_GdiPlusToken = 0;

    private const String GDIPLUS_NOT_INITIALIZED = "ERROR: Can not initialize GDI+ library.";
}