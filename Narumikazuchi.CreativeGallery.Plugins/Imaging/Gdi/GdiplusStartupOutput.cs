namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

[StructLayout(LayoutKind.Sequential)]
internal struct GdiplusStartupOutput
{
    static internal GdiplusStartupOutput MakeGdiplusStartupOutput()
    {
        GdiplusStartupOutput result = new()
        {
            NotificationHook = IntPtr.Zero,
            NotificationUnhook = IntPtr.Zero,
        };
        return result;
    }

    internal IntPtr NotificationHook;
    internal IntPtr NotificationUnhook;
}