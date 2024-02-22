namespace Narumikazuchi.CreativeGallery.Plugins.Imaging;

internal sealed class GdiPlusStreamHelper
{
    internal GdiPlusStreamHelper(Stream stream,
                                 Boolean seekToOrigin)
    {
        m_Buffer = new Byte[4096];
        m_Stream = stream;

        if (stream.CanSeek is true &&
            seekToOrigin is true)
        {
            stream.Seek(offset: 0,
                        origin: SeekOrigin.Begin);
        }
    }

    internal GdiPlus.StreamGetHeaderDelegate GetHeaderDelegate
    {
        get
        {
            return this.StreamGetHeader;
        }
    }

    internal GdiPlus.StreamGetBytesDelegate GetBytesDelegate
    {
        get
        {
            return this.StreamGetBytes;
        }
    }

    internal GdiPlus.StreamPutBytesDelegate PutBytesDelegate
    {
        get
        {
            return this.StreamPutBytes;
        }
    }

    internal GdiPlus.StreamSeekDelegate SeekDelegate
    {
        get
        {
            return this.StreamSeek;
        }
    }

    internal GdiPlus.StreamCloseDelegate CloseDelegate
    {
        get
        {
            return this.StreamClose;
        }
    }

    internal GdiPlus.StreamSizeDelegate SizeDelegate
    {
        get
        {
            return this.StreamSize;
        }
    }

    private Int32 StreamGetHeader(IntPtr buffer,
                                  Int32 bufferSize)
    {
        m_StartBuffer = new Byte[bufferSize];

        Int32 read;
        try
        {
            read = m_Stream.Read(buffer: m_StartBuffer.Value!,
                                 offset: 0,
                                 count: bufferSize);
        }
        catch (IOException)
        {
            return -1;
        }

        if (read > 0 &&
            buffer.Equals(other: IntPtr.Zero) is false)
        {
            Marshal.Copy(source: m_StartBuffer.Value!,
                         startIndex: 0,
                         destination: buffer,
                         length: read);
        }

        m_StartBufferPosition = 0;
        m_StartBufferLength = read;

        return read;
    }

    private Int32 StreamGetBytes(IntPtr buffer,
                                 Int32 bufferSize,
                                 Boolean peek)
    {
        if (buffer.Equals(other: IntPtr.Zero) is true &&
            peek is true)
        {
            return -1;
        }

        if (bufferSize > m_Buffer.Length)
        {
            m_Buffer = new Byte[bufferSize];
        }

        Int32 read = 0;
        Int64 position = 0;

        if (bufferSize > 0)
        {
            if (m_Stream.CanSeek)
            {
                position = m_Stream.Position;
            }

            if (m_StartBuffer.HasValue is true &&
                m_StartBufferLength > 0)
            {
                if (m_StartBufferLength > bufferSize)
                {
                    Array.Copy(sourceArray: m_StartBuffer.Value,
                               sourceIndex: m_StartBufferPosition,
                               destinationArray: m_Buffer,
                               destinationIndex: 0,
                               length: bufferSize);

                    m_StartBufferPosition += bufferSize;
                    m_StartBufferLength -= bufferSize;
                    read = bufferSize;
                    bufferSize = 0;
                }
                else
                {
                    Array.Copy(sourceArray: m_StartBuffer.Value,
                               sourceIndex: m_StartBufferPosition,
                               destinationArray: m_Buffer,
                               destinationIndex: 0,
                               length: m_StartBufferLength);

                    bufferSize -= m_StartBufferLength;
                    read = m_StartBufferLength;
                    m_StartBufferLength = 0;
                }
            }

            if (bufferSize > 0)
            {
                try
                {
                    read += m_Stream.Read(m_Buffer, read, bufferSize);
                }
                catch (IOException)
                {
                    return -1;
                }
            }

            if (read > 0 &&
                buffer.Equals(other: IntPtr.Zero) is false)
            {
                Marshal.Copy(source: m_Buffer,
                             startIndex: 0,
                             destination: buffer,
                             length: read);
            }

            if (m_Stream.CanSeek is false &&
                bufferSize is 10 &&
                peek is true)
            {
                // Special 'hack' to support peeking of the type for gdi+ on non-seekable streams
            }

            if (peek is true)
            {
                if (m_Stream.CanSeek is true)
                {
                    m_Stream.Seek(offset: position,
                                  origin: SeekOrigin.Begin);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        return read;
    }

    private Int64 StreamSeek(Int32 offset, Int32 whence)
    {
        if (whence is < 0 
                   or > 2)
        {
            return -1;
        }

        m_StartBufferPosition += m_StartBufferLength;
        m_StartBufferLength = 0;

        SeekOrigin origin;

        switch (whence)
        {
            case 0:
                origin = SeekOrigin.Begin;
                break;
            case 1:
                origin = SeekOrigin.Current;
                break;
            case 2:
                origin = SeekOrigin.End;
                break;
            default:
                return -1;
        }

        return m_Stream.Seek(offset: offset,
                             origin: origin);
    }

    private Int32 StreamPutBytes(IntPtr buffer, Int32 bufferSize)
    {
        if (bufferSize > m_Buffer.Length)
        {
            m_Buffer = new Byte[bufferSize];
        }

        Marshal.Copy(source: buffer,
                     destination: m_Buffer,
                     startIndex: 0,
                     length: bufferSize);

        m_Stream.Write(buffer: m_Buffer,
                       offset: 0,
                       count: bufferSize);

        return bufferSize;
    }

    private void StreamClose()
    {
        m_Stream.Dispose();
    }

    private Int64 StreamSize()
    {
        try
        {
            return m_Stream.Length;
        }
        catch
        {
            return -1;
        }
    }

    private Byte[] m_Buffer;
    private Int32 m_StartBufferPosition;
    private Int32 m_StartBufferLength;
    private Optional<Byte[]> m_StartBuffer = default;

    private readonly Stream m_Stream;
}