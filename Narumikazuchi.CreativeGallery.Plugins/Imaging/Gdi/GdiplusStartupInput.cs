namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

[StructLayout(LayoutKind.Sequential)]
internal struct GdiplusStartupInput
{
    static internal GdiplusStartupInput MakeGdiplusStartupInput()
    {
        GdiplusStartupInput result = new()
        {
            GdiplusVersion = 1,
            DebugEventCallback = IntPtr.Zero,
            SuppressBackgroundThread = 0,
            SuppressExternalCodecs = 0,
        };
        return result;
    }

    internal UInt32 GdiplusVersion;
    internal IntPtr DebugEventCallback;
    internal Int32 SuppressBackgroundThread;
    internal Int32 SuppressExternalCodecs;
}