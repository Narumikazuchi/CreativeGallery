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

            //if (_metafileHolder != null)
            //{
            //    var mh = _metafileHolder;
            //    _metafileHolder = null;
            //    mh.GraphicsDisposed();
            //}

            m_IsDisposed = true;
        }

        GC.SuppressFinalize(obj: this);
    }

    static internal Graphics FromImage(ImageFile image)
    {
        GdiPlusStatus status = GdiPlus.GdipGetImageGraphicsContext(handle: image.Handle,
                                                                   graphics: out IntPtr handle);
        GdiPlus.ThrowIfFailed(status: status);
        Graphics result = new(handle: handle,
                              image: image);
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

    private Graphics(IntPtr handle,
                     ImageFile image)
    {
        this.Handle = handle;
        if (image is Metafile metafile)
        {
            //_metafileHolder = metafile.AddMetafileHolder();
        }
    }

    ~Graphics()
    {
        this.Dispose();
    }

    private Boolean m_IsDisposed;
}