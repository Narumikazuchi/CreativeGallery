namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

internal sealed class Graphics :
    MarshalByRefObject,
    IDisposable
{
    public void Dispose()
    {
        GdiPlusStatus status;
        if (m_IsDisposed is false)
        {
            status = GdiPlus.GdipDeleteGraphics(handle: this.Handle);
            GdiPlus.ThrowIfFailed(status: status);

            m_IsDisposed = true;
        }

        GC.SuppressFinalize(obj: this);
    }

    static internal Graphics FromImage(Bitmap image)
    {
        GdiPlusStatus status = GdiPlus.GdipGetImageGraphicsContext(handle: image.Handle,
                                                                   graphics: out IntPtr handle);
        GdiPlus.ThrowIfFailed(status: status);
        Graphics result = new(handle: handle);
        if (GdiPlus.UseLinuxDraw is true)
        {
            Rectangle rectangle = new(x: 0,
                                      y: 0,
                                      width: image.Width,
                                      height: image.Height);
            GdiPlus.GdipSetVisibleClip_linux(handle: handle,
                                             rectangle: ref rectangle);
        }

        return result;
    }

    internal IntPtr Handle
    {
        get;
    }

    private Graphics(IntPtr handle)
    {
        this.Handle = handle;
    }

    ~Graphics()
    {
        this.Dispose();
    }

    private Boolean m_IsDisposed;
}