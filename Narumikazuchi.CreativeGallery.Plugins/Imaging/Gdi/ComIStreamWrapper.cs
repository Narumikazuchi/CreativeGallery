namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

internal sealed class ComIStreamWrapper : IStream
{
    public void Read(Byte[] pv,
                     Int32 cb,
                     IntPtr pcbRead)
    {
        Int32 read = 0;
        if (cb is not 0)
        {
            this.SetSizePosition();
            read = m_Stream.Read(buffer: pv,
                                 offset: 0,
                                 count: cb);
        }

        if (pcbRead.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.WriteInt32(ptr: pcbRead,
                               val: read);
        }
    }

    public void Write(Byte[] pv,
                      Int32 cb,
                      IntPtr pcbWritten)
    {
        if (cb is not 0)
        {
            this.SetSizePosition();
            m_Stream.Write(buffer: pv,
                           offset: 0,
                           count: cb);
        }

        if (pcbWritten.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.WriteInt32(ptr: pcbWritten,
                               val: cb);
        }
    }

    public void Seek(Int64 dlibMove,
                     Int32 dwOrigin,
                     IntPtr plibNewPosition)
    {
        Int64 length = m_Stream.Length;
        Int64 newPosition;

        switch ((SeekOrigin)dwOrigin)
        {
            case SeekOrigin.Begin:
                newPosition = dlibMove;
                break;
            case SeekOrigin.Current:
                if (m_Position is -1)
                {
                    newPosition = m_Stream.Position + dlibMove;
                }
                else
                {
                    newPosition = m_Position + dlibMove;
                }

                break;
            case SeekOrigin.End:
                newPosition = length + dlibMove;
                break;
            default:
                throw new Exception(message: $"HResult: {STG_E_INVALIDFUNCTION}");
        }

        if (newPosition > length)
        {
            m_Position = newPosition;
        }
        else
        {
            m_Stream.Position = newPosition;
            m_Position = -1;
        }

        if (plibNewPosition.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.WriteInt64(ptr: plibNewPosition,
                               val: newPosition);
        }
    }

    public void SetSize(Int64 libNewSize)
    {
        m_Stream.SetLength(value: libNewSize);
    }

    public void CopyTo(IStream pstm,
                       Int64 cb,
                       IntPtr pcbRead,
                       IntPtr pcbWritten)
    {
        Int32 read;
        Int32 count;
        Int64 written = 0;

        if (cb is not 0)
        {
            if (cb < 4096)
            {
                count = (Int32)cb;
            }
            else
            {
                count = 4096;
            }

            Byte[] buffer = new Byte[count];
            this.SetSizePosition();
            while (true)
            {
                read = m_Stream.Read(buffer: buffer,
                                     offset: 0,
                                     count: count);
                if (read is 0)
                {
                    break;
                }

                pstm.Write(pv: buffer,
                           cb: read,
                           pcbWritten: IntPtr.Zero);
                written += read;
                if (written >= cb)
                {
                    break;
                }

                if (cb - written < 4096)
                {
                    count = (Int32)(cb - written);
                }
            }
        }

        if (pcbRead.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.WriteInt64(ptr: pcbRead,
                               val: written);
        }

        if (pcbWritten.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.WriteInt64(ptr: pcbWritten,
                               val: written);
        }
    }

    public void Commit(Int32 grfCommitFlags)
    {
        m_Stream.Flush();
        this.SetSizePosition();
    }

    public void Revert()
    {
        throw new Exception(message: $"HResult: {STG_E_INVALIDFUNCTION}");
    }

    public void LockRegion(Int64 libOffset,
                           Int64 cb,
                           Int32 dwLockType)
    {
        throw new Exception(message: $"HResult: {STG_E_INVALIDFUNCTION}");
    }

    public void UnlockRegion(Int64 libOffset,
                             Int64 cb,
                             Int32 dwLockType)
    {
        throw new Exception(message: $"HResult: {STG_E_INVALIDFUNCTION}");
    }

    public void Stat(out STATSTG pstatstg,
                     Int32 grfStatFlag)
    {
        pstatstg = new STATSTG()
        {
            cbSize = m_Stream.Length,
        };
    }

    public void Clone(out IStream ppstm)
    {
        ppstm = default!;
        throw new Exception(message: $"HResult: {STG_E_INVALIDFUNCTION}");
    }

    internal ComIStreamWrapper(Stream stream)
    {
        m_Stream = stream;
    }

    private void SetSizePosition()
    {
        if (m_Position is not -1)
        {
            if (m_Position > m_Stream.Length)
            {
                m_Stream.SetLength(value: m_Position);
            }

            m_Stream.Position = m_Position;
            m_Position = -1;
        }
    }

    private Int64 m_Position = 0L;

    private readonly Stream m_Stream;

    private const Int32 STG_E_INVALIDFUNCTION = unchecked((Int32)0x80030001);
}