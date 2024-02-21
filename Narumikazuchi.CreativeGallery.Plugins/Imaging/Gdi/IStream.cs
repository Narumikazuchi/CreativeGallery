namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

[Guid("0000000c-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal partial interface IStream
{
    public void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1), Out] Byte[] pv,
                                Int32 cb,
                                IntPtr pcbRead);

    public void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Byte[] pv,
                      Int32 cb,
                      IntPtr pcbWritten);

    public void Seek(Int64 dlibMove,
                     Int32 dwOrigin,
                     IntPtr plibNewPosition);

    public void SetSize(Int64 libNewSize);

    public void CopyTo(IStream pstm,
                       Int64 cb,
                       IntPtr pcbRead,
                       IntPtr pcbWritten);

    public void Commit(Int32 grfCommitFlags);

    public void Revert();

    public void LockRegion(Int64 libOffset,
                           Int64 cb,
                           Int32 dwLockType);

    public void UnlockRegion(Int64 libOffset,
                             Int64 cb,
                             Int32 dwLockType);

    public void Stat(out STATSTG pstatstg,
                     Int32 grfStatFlag);

    public void Clone(out IStream ppstm);
}