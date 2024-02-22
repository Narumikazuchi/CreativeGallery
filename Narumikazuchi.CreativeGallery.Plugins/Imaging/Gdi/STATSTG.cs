namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct STATSTG
{
    public String pwcsName;
    public Int32 type;
    public Int64 cbSize;
    public FILETIME mtime;
    public FILETIME ctime;
    public FILETIME atime;
    public Int32 grfMode;
    public Int32 grfLocksSupported;
    public Guid clsid;
    public Int32 grfStateBits;
    public Int32 reserved;
}