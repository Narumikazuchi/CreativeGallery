namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

public sealed class Metafile : ImageFile
{
    internal Metafile(IntPtr handle) :
        base(handle: handle)
    { }
}